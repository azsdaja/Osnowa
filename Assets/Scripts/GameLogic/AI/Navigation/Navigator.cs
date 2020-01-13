namespace GameLogic.AI.Navigation
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.FOV;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Pathfinding;
	using UnityUtilities;

	// todo: list of improvements to add:
	// mark positions occupied for more than 1 turn as unwalkable
	// decide what to do when a position is walkable, but temporarily occupied by other character (move around?)
	// add options like: forbid recalculating; limit pathfinding scope (by closed nodes? by path length?); 
	//                   force straight line; force straight permissive line
	// add run away navigation here (currently implemented in RunAwayActivity)?
	// also todo: move follow ability to here from ApproachActivity?
	public class Navigator : INavigator
	{
		private readonly IPathfinder _pathfinder;
		private readonly IGrid _grid;
		private readonly INaturalLineCalculator _naturalLineCreator;
		private readonly IRasterLineCreator _rasterLineCreator;
		private readonly IUiFacade _uiFacade;
		private readonly IDebugPathPresenter _debugPathPresenter;

		public Navigator(IPathfinder pathfinder, IGrid grid, INaturalLineCalculator naturalLineCreator, 
			IRasterLineCreator rasterLineCreator, IUiFacade uiFacade)
		{
			_pathfinder = pathfinder;
			_grid = grid;
			_naturalLineCreator = naturalLineCreator;
			_rasterLineCreator = rasterLineCreator;
			_uiFacade = uiFacade;
			_debugPathPresenter = new DebugPathPresenter();
		}

		public virtual NavigationData GetNavigationData(Position startPosition, Position targetPosition)
		{
			if (!_grid.IsWalkable(targetPosition))
				return null;

			var navigationData = new NavigationData
			{
				Destination = targetPosition,
				RemainingStepsInCurrentSegment = new Stack<Position>(),
				LastStep = startPosition
			};

			if (startPosition == targetPosition)
				return navigationData;

			IList<Position> line = _rasterLineCreator.GetRasterLinePermissive(startPosition.x, startPosition.y, targetPosition.x,
				targetPosition.y, _grid.IsWalkable);

			bool rasterLineReachesTarget = line.Last() == targetPosition;
			if (rasterLineReachesTarget)
			{
				_debugPathPresenter.PresentStraight(line);
				// performance: if GetRasterLinePermissive returned a stack, we wouldn't need to allocate memory for the list
				navigationData.RemainingNodes = new[] {targetPosition}.ToList();
				return navigationData;
			}

			PathfindingResponse result = _pathfinder.FindJumpPointsWithJps(startPosition, targetPosition);
			if (result.Result != PathfindingResult.Success)
			{
				return null;
			}
			IEnumerable<Position> naturalJumpPoints = _naturalLineCreator.GetNaturalJumpPoints(result.Positions).Skip(1);
			navigationData.RemainingNodes = naturalJumpPoints.ToList();
			_debugPathPresenter.PresentNonStraight(navigationData.RemainingNodes);

			return navigationData;
		}

		public NavigationResult ResolveNextStep(NavigationData navigationData, Position currentPosition, out Position nextStep)
		{
			nextStep = PositionUtilities.Min;
			if (navigationData.Destination == currentPosition)
			{
				return NavigationResult.Finished;
			}

			bool pathIsCorrupted = !NavigationDataIsValid(currentPosition, navigationData);
			if (pathIsCorrupted)
			{
				#if UNITY_EDITOR
				//Debug.Log("path is corrupted. recalculating path");
				#endif
				return ResolveWithRecalculation(navigationData, currentPosition, ref nextStep);
			}

			if (!navigationData.RemainingStepsInCurrentSegment.Any())
			{
				IList<Position> naturalLineToWalk = _naturalLineCreator.GetFirstLongestNaturalLine(
					currentPosition, navigationData.RemainingNodes, _grid.IsWalkable);
				Position naturalNextNode = naturalLineToWalk.Last();
				bool naturalNextNodeIsSameAsNextNextNode = navigationData.RemainingNodes.Count > 1 
														   && navigationData.RemainingNodes[1] == naturalNextNode;
				if (naturalNextNodeIsSameAsNextNextNode)
					navigationData.RemainingNodes.RemoveAt(0);
				else
					navigationData.RemainingNodes[0] = naturalNextNode;
				navigationData.RemainingStepsInCurrentSegment = new Stack<Position>(naturalLineToWalk.Skip(1).Reverse());
			}

			Position lastStep = navigationData.LastStep;

			if (!navigationData.RemainingStepsInCurrentSegment.Any())
			{
				throw new InvalidOperationException($"Missing remaining steps. Current: {currentPosition}, navigation: {navigationData}");
			}
			nextStep = navigationData.RemainingStepsInCurrentSegment.Pop();
			bool actorWasDisplaced = currentPosition != lastStep;
			if (actorWasDisplaced)
			{
				bool actorWasDisplacedFarFromLastStep = !PositionUtilities.IsOneStep(lastStep - currentPosition);
				if (actorWasDisplacedFarFromLastStep)
				{
					#if UNITY_EDITOR
					//Debug.Log("actor was displaced far from last step. recalculating path");
					#endif
					return ResolveWithRecalculation(navigationData, currentPosition, ref nextStep);
				}

				bool shouldMoveAgainToLastPosition = !PositionUtilities.IsOneStep(nextStep - currentPosition);
				if (shouldMoveAgainToLastPosition)
				{
					navigationData.RemainingStepsInCurrentSegment.Push(nextStep);
					nextStep = lastStep;
				}
			}

			if (!_grid.IsWalkable(nextStep))
			{
				#if UNITY_EDITOR
				//Debug.Log("next step not walkable. recalculating path");
				#endif
				return ResolveWithRecalculation(navigationData, currentPosition, ref nextStep);
			}

			bool willBeNextNode = nextStep == navigationData.RemainingNodes[0];
			if (willBeNextNode)
			{
				navigationData.RemainingNodes.RemoveAt(0);
			}

			navigationData.LastStep = nextStep;
			return NavigationResult.InProgress;
		}

		private NavigationResult ResolveWithRecalculation(NavigationData navigationData, Position currentPosition,
			ref Position nextStep)
		{
			NavigationData recalculatedNavigationData = GetNavigationData(currentPosition, navigationData.Destination);
			if (recalculatedNavigationData == null)
			{
				return NavigationResult.Unreachable;
			}
			navigationData.OverwriteBy(recalculatedNavigationData);

			bool newNavigationDataIsValid = NavigationDataIsValid(currentPosition, navigationData);
			if (!newNavigationDataIsValid)
			{
				throw new InvalidOperationException("Navigation data after recalculation is still not valid.");
			}

			NavigationResult result = ResolveNextStep(navigationData, currentPosition, out nextStep);
			return result == NavigationResult.InProgress ? NavigationResult.InProgressWithRecalculation : result;
		}

		public List<Position> GetJumpPoints(Position startPosition, Position targetPosition)
		{
			var stopwatch = Stopwatch.StartNew();
			
			PathfindingResponse result = _pathfinder.FindJumpPointsWithJps(startPosition, targetPosition);

			if (result.Result != PathfindingResult.Success)
			{
				return null;
			}
			List<Position> jumpPointsToTarget = result.Positions.Skip(1).ToList();

			string logEntry = String.Format("Path from {0} to {1}: {2} steps, {3} ms.",
				startPosition, targetPosition, jumpPointsToTarget.Count, stopwatch.ElapsedMilliseconds);
			//_uiFacade.AddLogEntry(logEntry);

			return jumpPointsToTarget;
		}

		public List<Position> GetStraightPath(Position startPosition, Position targetPosition)
		{
			IList<Position> naturalLine 
				= _naturalLineCreator.GetFirstLongestNaturalLine(startPosition, new[] {targetPosition}, _grid.IsWalkable);

			return naturalLine.Last() == targetPosition ? naturalLine.ToList() : null;
		}

		private static bool NavigationDataIsValid(Position currentPosition, NavigationData navigationData)
		{
			return navigationData.RemainingStepsInCurrentSegment != null
			       && navigationData.Destination != Position.Zero
			       && navigationData.LastStep != Position.Zero
			       && navigationData.RemainingNodes != null
			       && navigationData.RemainingNodes.Any()
			       && navigationData.RemainingNodes.First() != currentPosition;
		}
	}
}