namespace Osnowa.Osnowa.Grid
{
	using System.Collections.Generic;
	using Unity.Tiles.Scripts;

	public interface IPositionFlagsResolver
	{
		void InitializePositionFlags();

		void SetFlagsAt(int x, int y, int tilesByIdsCount, OsnowaBaseTile[] tilesByIds, List<int> idsOfNotFoundTiles);
	}
}