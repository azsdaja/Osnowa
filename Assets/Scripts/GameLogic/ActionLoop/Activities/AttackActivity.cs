namespace GameLogic.ActionLoop.Activities
{
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Rng;
	using UnityUtilities;

	public class AttackActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly INavigator _navigator;

		private NavigationData _navigationData;
		private Position _lastTargetPosition;
		private readonly GameEntity _targetEntity;
		private readonly int _giveUpDistance;
		private readonly IRandomNumberGenerator _rng;
		private IGameConfig _gameConfig;
		private readonly IEntityDetector _entityDetector;

		public AttackActivity(IActionFactory actionFactory, INavigator navigator, int giveUpDistance, GameEntity targetEntity, string name, IRandomNumberGenerator rng, IGameConfig gameConfig, IEntityDetector entityDetector) 
			: base(name)
		{
			_targetEntity = targetEntity;
			_rng = rng;
			_gameConfig = gameConfig;
			_entityDetector = entityDetector;
			_giveUpDistance = giveUpDistance;
			_actionFactory = actionFactory;
			_navigator = navigator;

			_lastTargetPosition = targetEntity.position.Position;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			if (!_targetEntity.hasPosition)
			{
				return Succeed(entity);
			}
            // osnowatodo
			if (Position.Distance(_targetEntity.position.Position, entity.position.Position) >= _giveUpDistance)
			{
				return Fail(entity);
			}

			Position targetCurrentPosition = _targetEntity.position.Position;

			bool targetIsOneStepAway = PositionUtilities.IsOneStep(entity.position.Position - targetCurrentPosition);

			if (targetIsOneStepAway)
			{
				return new ActivityStep
				{
					State = ActivityState.InProgress,
					GameAction = _actionFactory.CreateAttackAction(entity, _targetEntity),
				};
			}

			if (_rng.Check(0.03f))
			{
				return Fail(entity, _actionFactory.CreatePassAction(entity, 3f));
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