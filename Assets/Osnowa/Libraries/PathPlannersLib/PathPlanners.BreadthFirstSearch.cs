namespace Libraries.PathPlannersLib
{
    using System;
    using System.Collections.Generic;

    public class BreadthFirstSearch : SearchAlgorithm
    {
        private Queue<Point> queue;
        private Functions.Function moveCostFunction; 
        public BreadthFirstSearch(bool[,] unpassableNodes) : base(unpassableNodes) { }
        public override List<Point> FindPath(Point start, Point goal, out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            return FindPath(start, goal, Functions.EuclidianDistanceJPS, out pathCost, out nodesOpen, out nodesClosed);

        }
        public List<Point> FindPath(Point start, Point goal, Functions.Function moveCostFunction, 
			out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            this.moveCostFunction = moveCostFunction;
            return base.FindPath(start, goal, out pathCost, out nodesOpen, out nodesClosed);
        }
        protected override void Initialize()
        {
            base.Initialize();
            queue = new Queue<Point>();
            queue.Enqueue(start);
            inOpenset[sx, sy] = true;
        }
        protected override bool ComputeShortestPath(out int nodesOpen, out int nodesClosed)
        {
			nodesOpen = 0;
			nodesClosed = 0;

			int cx, cy;
            Point current;
            int nx_lb, nx_ub, ny_lb, ny_ub;
            while (queue.Count != 0)
            {
                current = queue.Dequeue();
                cx = current.X;
                cy = current.Y;

                inOpenset[cx, cy] = false;
                inClosedset[cx, cy] = true;
	            ++nodesClosed;
                if (cx == gx && cy == gy)
                {
                    return true;
                }
                else
                {
                    nx_lb = Math.Max(cx - 1, 0);
                    nx_ub = Math.Min(cx + 1, mapWidth - 1);
                    ny_lb = Math.Max(cy - 1, 0);
                    ny_ub = Math.Min(cy + 1, mapHeight - 1);
                    for (int nx = nx_lb; nx <= nx_ub; nx++)
                    {
                        for (int ny = ny_lb; ny <= ny_ub; ny++)
                        {
                            if (_UnpassableNodes[nx, ny] == false && (cx == nx && cy == ny) == false &&
                                inClosedset[nx, ny] == false && inOpenset[nx, ny] == false)
                            {
                                parents[nx, ny].X = cx;
                                parents[nx, ny].Y = cy;
                                queue.Enqueue(new Point(nx, ny));
                                inOpenset[nx, ny] = true;
	                            ++nodesOpen;
                            }
                        }
                    }
                }
            }
            return false;
        }
        protected override List<Point> ReconstructPath(out uint pathCost)
        {
            var path = new List<Point>();
            var node = goal;
            Point parent;

            pathCost = 0;

            path.Add(node);
            while (node.Equals(start) == false)
            {
                parent = parents[node.X, node.Y];
                pathCost += moveCostFunction(parent.X, parent.Y, node.X, node.Y);
                path.Add(parent);
                node = parent;
            }
            return path;
        }
    }
}
