namespace Osnowa.Osnowa.Unity.Tiles
{
	using Osnowa.Tiles;
	using Scripts;

	public interface ITileByIdProvider
	{
		OsnowaBaseTile[] GetTilesByIds(Tileset tileset);
	}
}