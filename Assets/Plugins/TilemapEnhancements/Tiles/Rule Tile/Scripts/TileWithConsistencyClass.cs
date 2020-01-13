using UnityEngine;

namespace Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts
{
	[CreateAssetMenu]
	public class TileWithConsistencyClass : KafelkiTile
	{
		/// <summary>
		/// When more than 0, is treated by RuleTiles as Neighbor.This of all other tiles with same consistency class.
		/// </summary>
		public int ConsistencyClass;

		public bool IsConsistentWith(TileWithConsistencyClass other)
		{
			return ConsistencyClass > 0 && ConsistencyClass == other.ConsistencyClass;
		}
	}
}