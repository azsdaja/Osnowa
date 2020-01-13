namespace Osnowa.Osnowa.Unity
{
	using Example;

	public class TilemapInitializer : ITilemapInitializer
	{
		private readonly ISceneContext _sceneContext;

		public TilemapInitializer(ISceneContext sceneContext)
		{
			_sceneContext = sceneContext;
		}

		public void InitializeVisibilityOfTiles(Vision vision)
		{
			_sceneContext.UnseenMaskTilemap.gameObject.SetActive(vision != Vision.Permanent);
			_sceneContext.FogOfWarTilemap.gameObject.SetActive(vision == Vision.Undiscovered);
		}
	}
}