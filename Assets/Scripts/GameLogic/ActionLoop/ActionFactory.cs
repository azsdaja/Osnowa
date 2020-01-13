namespace GameLogic.ActionLoop
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Actions;
	using Entities;
	using GridRelated;
	using Osnowa;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Example.ECS;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.RNG;
	using Osnowa.Osnowa.Unity;
	using UI;

	public class ActionFactory : IActionFactory
	{
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;
		private readonly IGrid _grid;
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IRandomNumberGenerator _randomNumberGenerator;
		private readonly IDeathHandler _deathHandler;
		private readonly IActionEffectFactory _actionEffectFactory;
		private readonly ISceneContext _sceneContext;
		private readonly IGameConfig _gameConfig;
		private readonly IViewCreator _viewCreator;
		private readonly ITileMatrixUpdater _tileMatrixUpdater;
		private readonly IFirstPlaceInAreaFinder _firstPlaceInAreaFinder;
		private readonly IBroadcastStimulusSender _stimulusBroadcaster;
		private readonly IEntityDetector _entityDetector;
		private readonly LoadViewSystem _loadViewSystem;
		private readonly IUiFacade _uiFacade;
		private readonly IOsnowaContextManager _contextManager;
		private readonly IEntityViewBehaviourInitializer _entityViewBehaviourInitializer;
		private readonly IAggressionTriggerer _aggressionTriggerer;
		private readonly IEntityGenerator _entityGenerator;
        private readonly ReactiveFeature _reactiveFeature;

        public ActionFactory(IGrid grid, IActionEffectFactory actionEffectFactory, IUnityGridInfoProvider unityGridInfoProvider, IPositionEffectPresenter positionEffectPresenter, IRandomNumberGenerator randomNumberGenerator, IDeathHandler deathHandler, ISceneContext sceneContext, IGameConfig gameConfig, IViewCreator viewCreator, ITileMatrixUpdater tileMatrixUpdater, IFirstPlaceInAreaFinder firstPlaceInAreaFinder, IBroadcastStimulusSender stimulusBroadcaster, IEntityDetector entityDetector, LoadViewSystem loadViewSystem, IUiFacade uiFacade, IOsnowaContextManager contextManager, IEntityViewBehaviourInitializer entityViewBehaviourInitializer, IAggressionTriggerer aggressionTriggerer, IEntityGenerator entityGenerator, ReactiveFeature reactiveFeature)
		{
		    _grid = grid;
            _unityGridInfoProvider = unityGridInfoProvider;
			_positionEffectPresenter = positionEffectPresenter;
			_randomNumberGenerator = randomNumberGenerator;
			_deathHandler = deathHandler;
			_actionEffectFactory = actionEffectFactory;
			_sceneContext = sceneContext;
			_gameConfig = gameConfig;
			_viewCreator = viewCreator;
			_tileMatrixUpdater = tileMatrixUpdater;
			_firstPlaceInAreaFinder = firstPlaceInAreaFinder;
			_stimulusBroadcaster = stimulusBroadcaster;
			_entityDetector = entityDetector;
			_loadViewSystem = loadViewSystem;
			_uiFacade = uiFacade;
			_contextManager = contextManager;
			_entityViewBehaviourInitializer = entityViewBehaviourInitializer;
			_aggressionTriggerer = aggressionTriggerer;
			_entityGenerator = entityGenerator;
            _reactiveFeature = reactiveFeature;
        }

		public virtual IGameAction CreateDisplaceAction(GameEntity entity, GameEntity actorAtTargetPosition)
		{
			return new DisplaceAction(entity, actorAtTargetPosition, 1f, _actionEffectFactory);
		}

		public IGameAction CreateAttackAction(GameEntity entity, GameEntity attackedEntity)
		{
			return new AttackAction(entity,
				1f, _actionEffectFactory, _randomNumberGenerator, attackedEntity, _gameConfig, _aggressionTriggerer, _positionEffectPresenter, _contextManager, _reactiveFeature);
		}

		public virtual IGameAction CreateJustMoveAction(Position actionVector, GameEntity entity)
		{
			return new JustMoveAction(entity, 1f, _actionEffectFactory, actionVector, _grid);
		}

	    public virtual IGameAction CreateDropAction(GameEntity itemToDrop, GameEntity entity)
		{
			return new DropAction(entity, 1f, itemToDrop, _actionEffectFactory, _firstPlaceInAreaFinder, _entityDetector, _gameConfig, _uiFacade, _positionEffectPresenter);
		}

		public virtual IGameAction CreatePickUpAction(GameEntity itemToPickUp, GameEntity entity)
		{
			return new PickUpAction(entity, 1f, itemToPickUp, _actionEffectFactory, _entityDetector, _uiFacade);
		}

		public IGameAction CreateTakeToInventoryAction(GameEntity entity)
		{
			return new TakeToInventoryAction(entity, 0f, _actionEffectFactory);
		}

		public virtual IGameAction CreatePassAction(GameEntity entity, float cost = 1f)
		{
			Func<GameEntity, IEnumerable<IActionEffect>> inlineAction = actorDataParameter => Enumerable.Empty<IActionEffect>();
			return new LambdaAction(entity, cost, _actionEffectFactory, inlineAction);
		}

		public IGameAction CreateMoveAction(Position actionVector, GameEntity entity)
		{
			return new MoveAction(entity, 1f, _actionEffectFactory, actionVector, _grid);
		}

		public IGameAction CreateEatAction(GameEntity consumedEntity, GameEntity entity)
		{
			return new EatAction(entity, 1f, _actionEffectFactory, _positionEffectPresenter, consumedEntity);
		}

	    public IGameAction CreateLambdaAction(Func<GameEntity, IEnumerable<IActionEffect>> action, GameEntity entity, float cost = 1f)
		{
			return new LambdaAction(entity, cost, _actionEffectFactory, action);
		}

	    public IGameAction CreateKindapAction(GameEntity kidnappedEntity, GameEntity entity)
		{			return new KidnapAction(entity, 1f, kidnappedEntity, _actionEffectFactory);
		}

		public IGameAction CreateReleaseAction(GameEntity entity)
		{
			return new ReleaseAction(entity, 1f, _firstPlaceInAreaFinder, _actionEffectFactory);
		}

		public IGameAction CreateEatItemAction(GameEntity itemToEat, GameEntity entity)
		{
			return new EatItemAction(entity, 1f, itemToEat, _actionEffectFactory, _positionEffectPresenter);
		}

		public IGameAction CreateShoutWarningAction(GameEntity entity)
		{
			return new ShoutWarningAction(entity, 1f, _stimulusBroadcaster, _gameConfig, _positionEffectPresenter, _actionEffectFactory);
		}

		public IGameAction CreateTakeFromInventoryAction(GameEntity entity, int indexInInventory)
		{
			return new TakeFromInventoryAction(entity, indexInInventory, 0f, _actionEffectFactory, _loadViewSystem);
		}

		public IGameAction CreateSwapHandWithInventoryAction(GameEntity entity, int indexInInventory)
		{
			return new SwapHandWithInventoryAction(entity, indexInInventory, 1f, _actionEffectFactory, _loadViewSystem);
		}
	}
}