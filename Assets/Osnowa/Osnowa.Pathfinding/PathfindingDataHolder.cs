namespace Osnowa.Osnowa.Pathfinding
{
    using Core;
    using Libraries.PathPlannersLib;
    using Libraries.SpatialAStar.SpatialAStar.Algorithm;

    public class PathfindingDataHolder
    {
        private bool[,] _wallMatrixForJps;
        private Position _positionOffset;
        private MyPathNode[,] _pathNodeMatrixForSpatialAStar;

        public PathfindingDataHolder(int xSize, int ySize)
        {
            _wallMatrixForJps = new bool[xSize, ySize];

            _pathNodeMatrixForSpatialAStar = new MyPathNode[xSize, ySize];
            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                {
                    _pathNodeMatrixForSpatialAStar[x, y] = new MyPathNode(true, PositionToNonZeroBasedPosition(new Position(x, y)), 0f);
                }
        }

        public bool[,] WallMatrixForJps => _wallMatrixForJps;
        public Position PositionOffset => _positionOffset;
        internal MyPathNode[,] PathNodeMatrixForSpatialAStar => _pathNodeMatrixForSpatialAStar;

        public void UpdateWalkability(Position nonZeroBasedPosition, bool isWalkable)
        {
            UpdateWalkability(nonZeroBasedPosition, isWalkable ? 1f : 0f);
        }
        
        public void UpdateWalkability(Position nonZeroBasedPosition, float walkability)
        {
            Position zeroBasedPosition = PositionToNonZeroBasedPosition(nonZeroBasedPosition);

            bool isWalkable = walkability > 0;
			
            PathNodeMatrixForSpatialAStar[zeroBasedPosition.x, zeroBasedPosition.y].Cost = WalkCostForWalkability(walkability);
			
            PathNodeMatrixForSpatialAStar[zeroBasedPosition.x, zeroBasedPosition.y].SetIsWalkable(isWalkable);
            WallMatrixForJps[zeroBasedPosition.x, zeroBasedPosition.y] = !isWalkable;

        }
        
        /// <summary>
        /// Calculates the walk cost for walkability given in range 0.0-1.0. The higher is the cost, the less optimal is to walk through the position.
        /// </summary>
        protected virtual float WalkCostForWalkability(float walkability)
        {
            const float walkCostMultiplier = 5f; // empirical
            bool isWalkableThresholdPassed = walkability > 0.01f;
            if (isWalkableThresholdPassed)
            {
                return 1 + (1 - walkability)*walkCostMultiplier;
            }

            return -1;
        }
        
        public Position PositionToNonZeroBasedPosition(Position zeroBasedPosition)
        {
            return zeroBasedPosition + _positionOffset;
        }

        public Point PositionToZeroBasedPoint(Position nonZeroBasedPosition)
        {
            return new Point(nonZeroBasedPosition.x - _positionOffset.x, nonZeroBasedPosition.y - _positionOffset.y);
        }

        public Position PointToNonZeroBasedPosition(Point epGridPosition)
        {
            return new Position(epGridPosition.X + _positionOffset.x, epGridPosition.Y + _positionOffset.y);
        }
    }
}