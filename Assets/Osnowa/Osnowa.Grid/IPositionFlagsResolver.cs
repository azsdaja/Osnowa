namespace Osnowa.Osnowa.Grid
{
	using System.Collections.Generic;
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;

	public interface IPositionFlagsResolver
	{
		void InitializePositionFlags();

		void SetFlagsAt(int x, int y, int tilesByIdsCount, KafelkiTile[] tilesByIds, List<int> idsOfNotFoundTiles);
	}
}