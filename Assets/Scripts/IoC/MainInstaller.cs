namespace IoC
{
	using GameLogic.GameCore;
	using Osnowa.Osnowa.Context;
	using UI;
	using Zenject;
	using SceneContext = SceneContext;

	public class MainInstaller : MonoInstaller
	{
		public SceneContext SceneContext;
		public GameConfig GameConfig;
		public UiManager UiManager;
		public SavedComponents SavedComponents;

		public override void InstallBindings()	
		{
			GameGlobalsInstaller.Install(Container, SceneContext, GameConfig, UiManager, SavedComponents);
		}
	}
}
