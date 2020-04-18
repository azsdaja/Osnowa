namespace Osnowa.Osnowa.Unity.Tiles
{
	using System.Collections.Generic;
	using System.Linq;
	using Scripts;
	using UnityEngine;

	/// <summary>
	/// Finds all tiles
	/// </summary>
	public class TileByIdFromFolderProvider : ITileByIdProvider
	{
		private List<OsnowaBaseTile> _allTiles;

		public OsnowaBaseTile[] GetTilesByIds()
		{
			if (_allTiles == null)
			{
				Debug.Log("FETCHING ALL TILES - this should happen only once!");
				_allTiles = AssetLoader.LoadAll<OsnowaBaseTile>();
			}
			
			int maxId = _allTiles.Max(t => t.Id);

			var result = new OsnowaBaseTile[maxId + 1];
			foreach (OsnowaBaseTile tile in _allTiles)
			{
				result[tile.Id] = tile;
			}

			return result;
		}
	}
}