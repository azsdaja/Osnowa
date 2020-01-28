namespace Osnowa.Osnowa.Unity.Tiles
{
	using System.Collections.Generic;
	using System.Linq;
	using Scripts;
	using UnityEditor;

	/// <summary>
	/// Finds all tiles
	/// </summary>
	public class TileByIdFromFolderProvider : ITileByIdProvider
	{
		public OsnowaBaseTile[] GetTilesByIds()
		{
			List<OsnowaBaseTile> allTiles = AssetLoader.LoadAll<OsnowaBaseTile>();
			
			int maxId = allTiles.Max(t => t.Id);

			var result = new OsnowaBaseTile[maxId + 1];
			foreach (OsnowaBaseTile tile in allTiles)
			{
				result[tile.Id] = tile;
			}

			return result;
		}
	}
}