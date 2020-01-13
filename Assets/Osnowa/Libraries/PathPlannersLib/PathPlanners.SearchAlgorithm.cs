namespace Libraries.PathPlannersLib
{
	using System;
	using System.Collections.Generic;

	public abstract class SearchAlgorithm
    {
        protected bool[,] _UnpassableNodes;
        protected bool[,] inOpenset, inClosedset;
        protected Point[,] parents;
        protected Point start, goal;
        protected readonly int mapWidth, mapHeight;
        protected int sx, sy, gx, gy;
        protected const uint infinite = UInt32.MaxValue / 2;


        #region Public members
        public bool[,] UnpassableNodes
        {
            get { return _UnpassableNodes; }
        }
        public bool[,] InOpenSet
        {
            get { return inOpenset; }
        }
        public bool[,] InClosedSet
        {
            get { return inClosedset; }
        }
        public Size MapSize
        {
            get;
            private set;
        }
        #endregion

        #region Public methods
        public SearchAlgorithm(bool[,] unpassableNodes)
        {
            this._UnpassableNodes = unpassableNodes;
            mapWidth = unpassableNodes.GetLength(0);
            mapHeight = unpassableNodes.GetLength(1);
			MapSize = new Size(mapWidth, mapHeight);
        }
        public virtual List<Point> FindPath(Point start, Point goal, out int nodesOpen, out int nodesClosed)
        {
            uint pathCost = 0;
            return FindPath(start, goal, out pathCost, out nodesOpen, out nodesClosed);
        }
        public virtual List<Point> FindPath(Point start, Point goal, out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            this.start = start;
            this.goal = goal;

            sx = start.X;
            sy = start.Y;
            gx = goal.X;
            gy = goal.Y;

            pathCost = 0;
            Initialize();
            if (ComputeShortestPath(out nodesOpen, out nodesClosed)) return ReconstructPath(out pathCost);
            else return new List<Point>();
        }
        #endregion

        #region Private methods
        protected abstract bool ComputeShortestPath(out int nodesOpen, out int nodesClosed);
        protected abstract List<Point> ReconstructPath(out uint pathCost);
        protected virtual void Initialize()
        {
			if(inOpenset == null)
				inOpenset = new bool[mapWidth, mapHeight];
			else
			{
				Array.Clear(inOpenset, 0, inOpenset.Length);
			}

			if (inClosedset == null)
				inClosedset = new bool[mapWidth, mapHeight];
			else
			{
				Array.Clear(inClosedset, 0, inClosedset.Length);
			}

			if (parents == null)
				parents = new Point[mapWidth, mapHeight];
			else
			{
				Array.Clear(parents, 0, parents.Length);
			}
		}
        #endregion

    }
}
