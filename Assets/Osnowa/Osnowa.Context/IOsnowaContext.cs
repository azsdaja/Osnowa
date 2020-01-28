namespace Osnowa.Osnowa.Context
{
	using System.Collections.Generic;
	using Core;
	using Entities;
	using Pathfinding;

	public interface IOsnowaContext
	{
		PathfindingDataHolder PathfindingData { get; }
		PositionFlags PositionFlags { get; }
		MatrixByte[] TileMatricesByLayer { get; }
        MatrixFloat Walkability { get; }
	    Position StartingPosition { get; set; }
	    HashSet<IPositionedEntity> VisibleEntities { get; set; }
	    int TurnsPassed { get; set; }
	}
}