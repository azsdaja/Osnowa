namespace Osnowa.Osnowa.Unity.Tiles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Osnowa.Tiles;
	using Scripts;

	public class TileByIdProvider : ITileByIdProvider
	{
		public OsnowaBaseTile[] GetTilesByIds(Tileset tileset)
		{
			IList<OsnowaBaseTile> tilesDeclaredByName 
					= typeof(Tileset).GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => f.FieldType == typeof(OsnowaBaseTile))
					.Select(f => f.GetValue(tileset)).Cast<OsnowaBaseTile>().ToList();

			List<OsnowaBaseTile> allDeclaredTiles = tilesDeclaredByName.Union(tileset.OtherTiles).ToList();
			if (allDeclaredTiles.Any(t => t == null))
			{
				throw new InvalidOperationException("Some tileset in tileset are not assigned!");
			}

			int maxId = allDeclaredTiles.Max(t => t.Id);

			var result = new OsnowaBaseTile[maxId + 1];

			foreach (OsnowaBaseTile declaredTile in allDeclaredTiles)
			{
				result[declaredTile.Id] = declaredTile;
			}

			return result;
		}
	}
}