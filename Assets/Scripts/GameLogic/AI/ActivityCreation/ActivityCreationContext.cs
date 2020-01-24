namespace GameLogic.AI.ActivityCreation
{
	using ActionLoop;
	using Entities;
	using GridRelated;
	using Navigation;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.RNG;
	using UI;

	class ActivityCreationContext : IActivityCreationContext
	{
		public ActivityCreationContext(INavigator navigator, IRandomNumberGenerator rng, IActionFactory actionFactory, ISceneContext sceneContext, 
            IExampleContextManager contextManager, IGameConfig gameConfig, IEntityDetector entityDetector, ICalculatedAreaAccessor calculatedAreaAccessor, 
            IUiFacade uiFacade, IRasterLineCreator rasterLineCreator, IPositionEffectPresenter positionEffectPresenter, 
            IFriendshipResolver friendshipResolver, GameContext context)
		{
			Navigator = navigator;
			Rng = rng;
			ActionFactory = actionFactory;
			SceneContext = sceneContext;
			ContextManager = contextManager;
			GameConfig = gameConfig;
			EntityDetector = entityDetector;
			CalculatedAreaAccessor = calculatedAreaAccessor;
			UiFacade = uiFacade;
			RasterLineCreator = rasterLineCreator;
			PositionEffectPresenter = positionEffectPresenter;
			FriendshipResolver = friendshipResolver;
			Context = context;
		}

		public INavigator Navigator { get; }
		public IRandomNumberGenerator Rng { get; }
		public IActionFactory ActionFactory { get; }
		public ISceneContext SceneContext { get; }
		public IExampleContextManager ContextManager { get; }
		public IGameConfig GameConfig { get; }
		public IEntityDetector EntityDetector { get; }
		public ICalculatedAreaAccessor CalculatedAreaAccessor { get; }
		public IRasterLineCreator RasterLineCreator { get; }
		public IPositionEffectPresenter PositionEffectPresenter { get; }
		public IFriendshipResolver FriendshipResolver { get; }
		public GameContext Context { get; }
		public IUiFacade UiFacade { get; }
	}
}