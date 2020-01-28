namespace GameLogic.ActionLoop.Activities
{
	using System.Collections.Generic;
	using System.Linq;
	using AI.Navigation;
	using GridRelated;
	using Osnowa;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;
	using UnityUtilities;

	public class KeepDistanceActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly int _preferredDistance;
		private readonly INavigator _navigator;

		private NavigationData _navigationDataToGoodPosition;
		private readonly GameEntity _targetToKeepDistanceTo;
		private int _turnsPassedToRecalculate;
		private Position _lastTargetPosition;
		
		private readonly ICalculatedAreaAccessor _calculatedAreaAccessor;
		private readonly IRandomNumberGenerator _rng;
		private readonly IOsnowaContextManager _contextManager;

		public KeepDistanceActivity(IActionFactory actionFactory, int preferredDistance, INavigator navigator, 
			GameEntity targetToKeepDistanceTo, ICalculatedAreaAccessor calculatedAreaAccessor, IRandomNumberGenerator rng, string name, IOsnowaContextManager contextManager) 
			: base(name)
		{
			_actionFactory = actionFactory;
			_preferredDistance = preferredDistance;
			_navigator = navigator;
			_targetToKeepDistanceTo = targetToKeepDistanceTo;
			
			_calculatedAreaAccessor = calculatedAreaAccessor;
			_rng = rng;
			_contextManager = contextManager;

			_lastTargetPosition = targetToKeepDistanceTo.position.Position;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			float chanceToStop = entity.isAggressive ? 0.2f : 0.07f;
			if (!_targetToKeepDistanceTo.hasPosition || _rng.Check(chanceToStop))
			{
				return Succeed(entity);
			}
		    int turnsPassed = 1; // osnowatodo Contexts.sharedInstance.game.turMinelo.Tur;

			bool targetPositionHasChanged = _targetToKeepDistanceTo.position.Position != _lastTargetPosition;
			float chanceToKeepPosition = 0.8f;
			bool shouldKeepPosition = !targetPositionHasChanged && _rng.Check(chanceToKeepPosition);
			if (shouldKeepPosition)
			{
				return KeepPosition(entity);
			}

			bool recalculateCooldownHasPassed = turnsPassed >= _turnsPassedToRecalculate;
			bool shouldRecalculate = (_navigationDataToGoodPosition == null) 
				|| (recalculateCooldownHasPassed && (targetPositionHasChanged || _rng.Check(0.2f)));
			if (shouldRecalculate)
			{
				_turnsPassedToRecalculate = turnsPassed + 3;
				_lastTargetPosition = _targetToKeepDistanceTo.position.Position;

				IFloodArea targetFlood = _calculatedAreaAccessor.FetchWalkableFlood(_lastTargetPosition, _preferredDistance);
				_navigationDataToGoodPosition = FindNavigationDataToGoodPosition(entity, targetFlood, _preferredDistance);
				if (_navigationDataToGoodPosition == null)
					return Fail(entity);
			}
			
			Position nextStep;
			NavigationResult navigationResult = _navigator.ResolveNextStep(_navigationDataToGoodPosition, entity.position.Position, out nextStep);
			if (navigationResult == NavigationResult.Finished)
			{
				return new ActivityStep
				{
					GameAction = _actionFactory.CreatePassAction(entity),
					State = ActivityState.InProgress
				};
			}
			if (nextStep == PositionUtilities.Min)
			{
				return Fail(entity);
			}

			IGameAction moveGameAction = CreateMoveAction(entity, nextStep);

			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = moveGameAction
			};
		}

		private ActivityStep KeepPosition(GameEntity entity)
		{
			IGameAction action = _actionFactory.CreatePassAction(entity);
			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = action
			};
		}

		private NavigationData FindNavigationDataToGoodPosition(GameEntity entity, IFloodArea targetFlood, int preferredDistance)
		{
			IList<Position> availablePositions = GetPositionsOnFloodWithGivenDistance(targetFlood, preferredDistance).ToList();

			int attemptsLimit = 5;
			int attempts = 0;
			while (true)
			{
				++attempts;
				if (attempts > attemptsLimit)
				{
					Debug.Log("Failed after " + attempts + " attempts.");
					return null;
				}
				Position[] candidates = {_rng.Choice(availablePositions), _rng.Choice(availablePositions), _rng.Choice(availablePositions)};
				var navigationsToCandidates = new List<NavigationData>(candidates.Length);

				foreach (Position candidatePosition in candidates)
				{
					NavigationData navigation = _navigator.GetNavigationData(entity.position.Position, candidatePosition);
					navigationsToCandidates.Add(navigation);
				}

				List<NavigationData> validCandidates = navigationsToCandidates.Where(c => c != null).ToList();
				if (!validCandidates.Any())
				{
					return null;
				}

				validCandidates.Sort((first, second) =>
								PositionUtilities.WalkDistance(entity.position.Position, first.Destination)
									.CompareTo(PositionUtilities.WalkDistance(entity.position.Position, second.Destination)));
				NavigationData closestValidCandidate = validCandidates.First();
				return closestValidCandidate;
			}
		}

		private IEnumerable<Position> GetPositionsOnFloodWithGivenDistance(IFloodArea targetFlood, int preferredDistance)
		{
			foreach (Position position in targetFlood.Bounds.AllPositions())
			{
				int value = targetFlood.GetValueAtPosition(position);
				if (value == preferredDistance)
				{
					yield return position;
				}
			}
		}

		private IGameAction CreateMoveAction(GameEntity entity, Position nextStep)
		{
			Position direction = nextStep - entity.position.Position;
			IGameAction moveGameAction = _actionFactory.CreateJustMoveAction(direction, entity);
			return moveGameAction;
		}
	}
}