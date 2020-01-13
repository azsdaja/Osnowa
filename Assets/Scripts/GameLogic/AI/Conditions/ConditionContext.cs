namespace GameLogic.AI.Conditions
{
	using Entities;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Grid;

	public class ConditionContext : IConditionContext
	{
		public ConditionContext(IGameConfig gameConfig, ISceneContext sceneContext,
			IEntityDetector entityDetector, GameContext context, IFriendshipResolver friendshipResolver,
			IExampleContextManager contextManager)
		{
			GameConfig = gameConfig;
			SceneContext = sceneContext;
			EntityDetector = entityDetector;
			Context = context;
			FriendshipResolver = friendshipResolver;
			ContextManager = contextManager;
		}

		public IGameConfig GameConfig { get; }
		public IExampleContextManager ContextManager { get; }
		public ISceneContext SceneContext { get; }
		public IEntityDetector EntityDetector { get; }
		public GameContext Context { get; }
		public IFriendshipResolver FriendshipResolver { get; }
	}
}