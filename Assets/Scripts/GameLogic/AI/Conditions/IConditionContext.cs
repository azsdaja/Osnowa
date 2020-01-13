namespace GameLogic.AI.Conditions
{
	using Entities;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Grid;

	public interface IConditionContext
	{
		IGameConfig GameConfig { get; }
		IExampleContextManager ContextManager { get; }
		ISceneContext SceneContext { get; }
		IEntityDetector EntityDetector { get; }
		GameContext Context { get; }
		IFriendshipResolver FriendshipResolver { get; }
	}
}