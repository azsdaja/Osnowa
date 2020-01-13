namespace Osnowa.Osnowa.Pathfinding
{
	using System;
	using Core;

	/// <summary>
	/// Finds paths between points on the grid.
	/// </summary>
	public interface IPathfinder
	{
		/// <summary>
		/// Finds path using Jump Point Search algorithm. Generally very fast, but current implementation uses some memory for big and complicated maps.
		/// Does NOT take cell walk cost into consideration.
		/// </summary>
		PathfindingResponse FindJumpPointsWithJps(Position startPosition, Position targetPosition, JpsMode mode = JpsMode.AllowDiagonalBetweenWalls);

		/// <summary>
		/// Finds path using A* algorithm. 3 to 5 times slower than JPS, but uses very small amount of memory even for complicated maps.
		/// Does take cell walk cost into consideration.
		/// </summary>
		PathfindingResponse FindJumpPointsWithSpatialAstar(Position startPoint, Position targetPoint);

		PathfindingResponse FindFullPathWithSpatialAstar(Position startPosition, Position targetPosition);
	}
}
