namespace Osnowa.Osnowa.Unity.Tiles
{
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using Osnowa.Tiles;

	public interface ITileByIdProvider
	{
		OsnowaBaseTile[] GetTilesByIds(Tileset tileset);
	}
}