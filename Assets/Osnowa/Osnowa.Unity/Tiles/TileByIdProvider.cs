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
		public KafelkiTile[] GetTilesByIds(Tileset tileset)
		{
			IList<KafelkiTile> tilesDeclaredByName 
					= typeof(Tileset).GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => f.FieldType == typeof(KafelkiTile))
					.Select(f => f.GetValue(tileset)).Cast<KafelkiTile>().ToList();

			List<KafelkiTile> allDeclaredTiles = tilesDeclaredByName.Union(tileset.OtherTiles).ToList();
			if (allDeclaredTiles.Any(t => t == null))
			{
				throw new InvalidOperationException("Some tileset in tileset are not assigned!");
			}

			int maxId = allDeclaredTiles.Max(t => t.Id);

			var result = new KafelkiTile[maxId + 1];

			foreach (KafelkiTile declaredTile in allDeclaredTiles)
			{
				result[declaredTile.Id] = declaredTile;
			}

			return result;
		}
	}
}