namespace GameLogic.ActionLoop.Activities
{
	using System;
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UnityUtilities;

	public class ApproachActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly INavigator _navigator;

		private NavigationData _navigationData;
		private readonly Func<Position> _targetPositionGetter;
		private readonly int _turnsLimit;
		private Position _lastTargetPosition;

		public ApproachActivity(IActionFactory actionFactory, INavigator navigator, Func<Position> targetPositionGetter, int turnsLimit, string name) 
			: base(name)
		{
			_actionFactory = actionFactory;
			_navigator = navigator;
			_targetPositionGetter = targetPositionGetter;
			_turnsLimit = turnsLimit;

			_lastTargetPosition = targetPositionGetter();
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			Position targetCurrentPosition = _targetPositionGetter();

			bool reachedTarget = PositionUtilities.IsOneStep(entity.position.Position - targetCurrentPosition);
			if (reachedTarget)
			{
				return Succeed(entity);
			}

			bool targetPositionHasChanged = targetCurrentPosition != _lastTargetPosition;
			if (targetPositionHasChanged)
			{
				_lastTargetPosition = targetCurrentPosition;
			}
			if (targetPositionHasChanged || _navigationData == null)
			{
				// performance: should in fact be done every couple of turns
				_navigationData = _navigator.GetNavigationData(entity.position.Position, targetCurrentPosition);
			}

			Position nextStep;
			NavigationResult navigationResult = _navigator.ResolveNextStep(_navigationData, entity.position.Position, out nextStep);

			if (navigationResult == NavigationResult.Finished)
			{
				return Succeed(entity);
			}

			IGameAction moveGameAction = CreateMoveAction(nextStep, entity);

			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = moveGameAction
			};
		}

		private IGameAction CreateMoveAction(Position nextStep, GameEntity entity)
		{
			Position direction = nextStep - entity.position.Position;
			IGameAction moveGameAction = _actionFactory.CreateJustMoveAction(direction, entity);
			return moveGameAction;
		}
	}
}