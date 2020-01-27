namespace Osnowa.Osnowa.Unity.Tiles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using Osnowa.Tiles;

	public class TileByIdProvider : ITileByIdProvider
	{
		public OsnowaTile[] GetTilesByIds(Tileset tileset)
		{
			IList<OsnowaTile> tilesDeclaredByName 
					= typeof(Tileset).GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => f.FieldType == typeof(OsnowaTile))
					.Select(f => f.GetValue(tileset)).Cast<OsnowaTile>().ToList();

			List<OsnowaTile> allDeclaredTiles = tilesDeclaredByName.Union(tileset.OtherTiles).ToList();
			if (allDeclaredTiles.Any(t => t == null))
			{
				throw new InvalidOperationException("Some tileset in tileset are not assigned!");
			}

			int maxId = allDeclaredTiles.Max(t => t.Id);

			var result = new OsnowaTile[maxId + 1];

			foreach (OsnowaTile declaredTile in allDeclaredTiles)
			{
				result[declaredTile.Id] = declaredTile;
			}

			return result;
		}
	}
}