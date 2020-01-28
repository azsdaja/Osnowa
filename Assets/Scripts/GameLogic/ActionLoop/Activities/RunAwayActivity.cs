namespace GameLogic.ActionLoop.Activities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GridRelated;
	using Osnowa;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using UnityEngine;
	using UnityUtilities;

	public class RunAwayActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly ICalculatedAreaAccessor _calculatedAreaAccessor;
		private readonly Position _floodSource;
		private FollowStepsActivity _followStepsActivity;

		public RunAwayActivity(IActionFactory actionFactory, ICalculatedAreaAccessor calculatedAreaAccessor, Position floodSource, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_calculatedAreaAccessor = calculatedAreaAccessor;
			_floodSource = floodSource;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			if (_followStepsActivity != null)
			{
				ActivityStep goToResult = _followStepsActivity.CheckAndResolveStep(entity);
				if(goToResult.State != ActivityState.FinishedFailure)
					return goToResult;
			}

			int escapeFloodRange = entity.vision.VisionRange;
			int allowedDelay = 3;

			IFloodArea enemyFlood = _calculatedAreaAccessor.FetchWalkableFlood(_floodSource, escapeFloodRange);

			Position startingPosition = entity.position.Position;
			bool standingOutsideOfFlood = enemyFlood.GetValueAtPosition(startingPosition) == int.MaxValue;
			if (standingOutsideOfFlood)
			{
				return new ActivityStep
				{
					GameAction = _actionFactory.CreatePassAction(entity),
					State = ActivityState.InProgress
				};
			}
			Position targetPosition;
			var predecessors = FindBestPositionInFloodToFleeTo(entity, startingPosition, allowedDelay, enemyFlood, out targetPosition);
			Stack<Position> steps = new Stack<Position>();
			CreateStepsStack(targetPosition, steps, predecessors);
			if(!steps.Any())
				Debug.LogError($"can't find best flood position starting at {startingPosition}.");

			_followStepsActivity = new FollowStepsActivity(_actionFactory, steps, "Run away — steps");

			return _followStepsActivity.CheckAndResolveStep(entity);
		}

		private Dictionary<Position, Position> FindBestPositionInFloodToFleeTo(GameEntity entity, Position startingPosition,
			int allowedDelay, IFloodArea enemyFlood, out Position targetPosition)
		{
			var predecessors = new Dictionary<Position, Position>();
			var delays = new Dictionary<Position, int> {[startingPosition] = 0};
			var openPositions = new Queue<Position>();
			Position[] bestPositionsForDelays = new Position[allowedDelay + 1];
			int[] bestScoresForDelays = new int[allowedDelay + 1];
			for (int i = 0; i < allowedDelay + 1; i++)
			{
				bestScoresForDelays[i] = Int32.MinValue/2;
			}

			openPositions.Enqueue(entity.position.Position);

			do
			{
				Position currentPosition = openPositions.Dequeue();
				int currentDelay = delays[currentPosition];
				int currentPositionScore = enemyFlood.GetValueAtPosition(currentPosition);

				ProcessNeighbours(currentPosition, delays, enemyFlood, currentPositionScore, currentDelay, allowedDelay, predecessors,
					openPositions, bestScoresForDelays, bestPositionsForDelays);
			} while (openPositions.Any());

			int bestDelayAdjustedScore = -int.MaxValue;
			int optimalDelayIndex = FindBestDelayCategoryOfPositions(allowedDelay, bestScoresForDelays, bestDelayAdjustedScore);

			targetPosition = bestPositionsForDelays[optimalDelayIndex];
			return predecessors;
		}

		private static int FindBestDelayCategoryOfPositions(int allowedDelay, int[] bestScoresForDelays,
			int bestDelayAdjustedScore)
		{
			int optimalDelayIndex = 0;
			for (int i = 0; i < allowedDelay; i++)
			{
				int delayAdjustedScore = bestScoresForDelays[i] - 2*i;
				if (delayAdjustedScore > bestDelayAdjustedScore)
				{
					bestDelayAdjustedScore = delayAdjustedScore;
					optimalDelayIndex = i;
				}
			}
			return optimalDelayIndex;
		}

		private static void CreateStepsStack(Position targetPosition, Stack<Position> steps, Dictionary<Position, Position> predecessors)
		{
			Position currentPosition = targetPosition;
			do
			{
				steps.Push(currentPosition);
			} while (predecessors.TryGetValue(currentPosition, out currentPosition));
			steps.Pop();
		}

		private static void ProcessNeighbours(Position currentNode, Dictionary<Position, int> delays, IFloodArea enemyFlood,
			int currentNodeScore, int currentDelay, int allowedDelay, Dictionary<Position, Position> predecessors, Queue<Position> openPositions,
			int[] bestScoresForDelays, Position[] bestPositionsForDelays)
		{
			foreach (Position neighbour in PositionUtilities.Neighbours8(currentNode)
				.Where(n => !delays.ContainsKey(n)))
			{
				int neighbourScore = enemyFlood.GetValueAtPosition(neighbour);
				if (neighbourScore == int.MaxValue)
				{
					continue;
				}
				int neighbourDelay = currentNodeScore - neighbourScore + 1 + currentDelay;
				if (neighbourDelay > allowedDelay)
					continue;

				delays[neighbour] = neighbourDelay;
				predecessors[neighbour] = currentNode;
				openPositions.Enqueue(neighbour);
				if (neighbourScore > bestScoresForDelays[neighbourDelay])
				{
					bestScoresForDelays[neighbourDelay] = neighbourScore;
					bestPositionsForDelays[neighbourDelay] = neighbour;
				}
			}
		}
	}
}