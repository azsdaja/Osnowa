using System.Collections.Generic;

namespace Osnowa.Osnowa.Grid
{
	using Core;

	public interface IEntityPresenter
	{       
		/// <summary>
		/// Updates the game entities, so that entities that are no longer visible are hidden and the ones that become visible are shown.
		/// </summary>
	    void UpdateVisibility(HashSet<Position> visiblePositions, Position observerPosition, int cellsInVisionRange);
	}
}