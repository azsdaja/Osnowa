namespace GameLogic.GridRelated
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;
	using UnityEngine.Tilemaps;

	public interface ITileMatrixUpdater
	{
		void Set(Position position, OsnowaBaseTile baseTileToSet);
		void RefreshTile(Position position, int layer);
		void ClearForTile(OsnowaBaseTile tileToCleanItsLayer);
		void Set(Tilemap tilemap, Position position, OsnowaBaseTile baseTileToSet);
		void SetMany(Tilemap tilemap, HashSet<Position> totalVisibleByThem, OsnowaBaseTile tilesetVisibleByOthers);
	}
}