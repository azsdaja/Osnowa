namespace Osnowa.Osnowa.Unity.Tiles
{
	using Scripts;

	public interface ITileByIdProvider
	{
		OsnowaBaseTile[] GetTilesByIds();
	}
}