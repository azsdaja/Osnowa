namespace Osnowa.Osnowa.Pathfinding
{
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// Displays paths with their costs on the grid. Dedicated for debugging purposes.
	/// </summary>
	public interface IPathRenderer
	{
		void ShowPath(IList<Position> pathPoints, float score);
		void ShowNaturalWay(List<Position> jumpPoints, float score);
	}
}