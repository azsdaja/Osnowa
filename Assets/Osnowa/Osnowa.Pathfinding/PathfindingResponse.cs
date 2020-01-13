namespace Osnowa.Osnowa.Pathfinding
{
	using System.Collections.Generic;
	using System.Linq;
	using Core;

	public class PathfindingResponse
	{
		public PathfindingResponse(PathfindingResult result)
		{
			Result = result;
		}

		public PathfindingResponse(List<Position> positions)
		{
			Positions = positions;
			Result = positions.Any() ? PathfindingResult.Success : PathfindingResult.FailureTargetUnreachable;
		}

		public PathfindingResponse(PathfindingResult result, List<Position> positions, int nodesOpen, int nodesClosed)
		{
			Result = result;
			Positions = positions;
			NodesOpen = nodesOpen;
			NodesClosed = nodesClosed;
		}

		public PathfindingResult Result { get; }
		public List<Position> Positions { get; }
		public int NodesOpen { get; }
		public int NodesClosed { get; }
	}
}