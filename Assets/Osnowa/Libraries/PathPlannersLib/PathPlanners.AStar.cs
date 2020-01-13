namespace Libraries.PathPlannersLib
{
	using System;
	using System.Collections.Generic;

	public class AStar : BestFirstSearch
    {
        protected uint[,] fMap, gMap;
        protected Functions.Function moveCostFunction, heuristicFunction; 

        #region Public methods
        public AStar(bool[,] unpassableNodes) : base(unpassableNodes) { }
        public override List<Point> FindPath(Point start, Point goal, out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            this.heuristicFunction = Functions.ManhattanDistance;
            this.moveCostFunction = Functions.EuclidianDistanceSingle;
            return base.FindPath(start, goal, out pathCost, out nodesOpen, out nodesClosed);
        }

        public List<Point> FindPath(Point start, Point goal, Functions.Function heuristicCostFunction, Functions.Function moveCostFunction, 
			out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            this.heuristicFunction = heuristicCostFunction;
            this.moveCostFunction = moveCostFunction;
	        return base.FindPath(start, goal, out pathCost, out nodesOpen, out nodesClosed);
        }
        #endregion

        #region Private methods
        protected override bool ComputeShortestPath(out int nodesOpen, out int nodesClosed)
        {
	        nodesOpen = 0;
	        nodesClosed = 0;

			int cx, cy;
            Point current;
            while (!openset.IsEmpty())
            {
                current = openset.Dequeue();
                cx = current.X;
                cy = current.Y;

                inOpenset[cx, cy] = false;
                if (cx == gx && cy == gy)
                {
                    return true;
                }
                else
                {
	                ++nodesClosed;
                    inClosedset[cx, cy] = true;
					nodesOpen += WorkSuccessors(cx, cy);
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

        protected override void Initialize()
        {
            base.Initialize();
	        if (fMap == null)
	        {
		        fMap = new uint[mapWidth, mapHeight];
	        }
	        else
	        {
				Array.Clear(fMap, 0, fMap.Length);
			}
	        if (gMap == null)
	        {
		        gMap = new uint[mapWidth, mapHeight];
	        }
	        else
	        {
				Array.Clear(fMap, 0, fMap.Length);
			}
            fMap.SetGrid(infinite);
            gMap.SetGrid(infinite);

            openset.Enqueue(0, start);
            inOpenset[sx, sy] = true;
            gMap[sx, sy] = 0;
            fMap[sx, sy] = heuristicFunction(sx, sy, gx, gy);
        }        
        protected virtual int WorkSuccessors(int cx, int cy)
        {
	        int openedNodes = 0;
            var nx_lb = Math.Max(cx - 1, 0);
            var nx_ub = Math.Min(cx + 1, mapWidth - 1);
            var ny_lb = Math.Max(cy - 1, 0);
            var ny_ub = Math.Min(cy + 1, mapHeight - 1);
            for (int nx = nx_lb; nx <= nx_ub; nx++)
            {
                for (int ny = ny_lb; ny <= ny_ub; ny++)
                {
                    if ((cx == nx && cy == ny) == false && _UnpassableNodes[nx, ny] == false)
                    {
                        var tentative_g_score = gMap[cx, cy] + moveCostFunction(cx, cy, nx, ny);
                        var current_g_score = gMap[nx, ny];
                        if (inClosedset[nx, ny] == true)
                        {
                            if (tentative_g_score >= current_g_score) continue;
                        }
                        var node_not_in_openset = inOpenset[nx, ny] == false;
                        if (node_not_in_openset || tentative_g_score < current_g_score)
                        {
                            var new_cost = tentative_g_score + heuristicFunction(nx, ny, gx, gy);
                            parents[nx, ny].X = cx;
                            parents[nx, ny].Y = cy;
                            gMap[nx, ny] = tentative_g_score;
                            fMap[nx, ny] = new_cost;
                            if (node_not_in_openset)
                            {
                                openset.Enqueue(new_cost, new Point(nx, ny));
                                inOpenset[nx, ny] = true;
	                            ++openedNodes;
                            }
                        }
                    }
                }
			}
			return openedNodes;
		}
		#endregion
	}
}
