namespace GameLogic.AI.ActivityCreation
{
	using ActionLoop;
	using Entities;
	using GridRelated;
	using Navigation;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.FOV;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.RNG;
	using UI;

	public interface IActivityCreationContext
	{
		INavigator Navigator { get; }
		IRandomNumberGenerator Rng { get; }
		IActionFactory ActionFactory { get; }
		ISceneContext SceneContext { get; }
		IExampleContextManager ContextManager { get; }
		IGameConfig GameConfig { get; }
		IEntityDetector EntityDetector { get; }
		ICalculatedAreaAccessor CalculatedAreaAccessor { get; }
		IUiFacade UiFacade { get; }
		IRasterLineCreator RasterLineCreator { get; }
		IPositionEffectPresenter PositionEffectPresenter { get; }
		IFriendshipResolver FriendshipResolver { get; }
		GameContext Context { get; }
	}
}