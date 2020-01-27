namespace GameLogic.GridRelated
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;

	public interface ITileMatrixUpdater
	{
		void Set(Position position, OsnowaBaseTile baseTileToSet);
		void RefreshTile(Position position, int layer);
	}
}