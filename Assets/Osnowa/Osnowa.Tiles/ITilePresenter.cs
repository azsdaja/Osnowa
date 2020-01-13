namespace Osnowa.Osnowa.Tiles
{
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// Manipulates the tilemaps to reflect the changes in visibility of given tiles.
	/// </summary>
	public interface ITilePresenter
	{
		/// <summary>
		/// Updates the presentable tilemaps using visiblePositions set, so that tiles that are no longer visible
		/// are dimmed and tiles that become visible are lit.
		/// </summary>
		void UpdateVisibility(HashSet<Position> visiblePositions);

		void ShortenHighTiles(Position playerPosition, int range);
		void ResetToHighTiles();
	}
}