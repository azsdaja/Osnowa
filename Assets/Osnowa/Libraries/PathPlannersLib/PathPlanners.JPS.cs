namespace Libraries.PathPlannersLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>
	/// Jump Point Search. This variant doesn't allow moving diagonally between blocks. If you need one that does, use <see cref="JpsTightDiagonal"/>.
	/// </summary>
	public class Jps : AStar
    {
        protected Point null_point = new Point(-1, -1);
        protected Point unset_point = new Point(-2, -2);
	    private List<Point> _neighborhoodList;

	    #region Public methods
        public Jps(bool[,] unpassableNodes) : base(unpassableNodes)
        {
        }

        public override List<Point> FindPath(Point start, Point goal, out int nodesOpen, out int nodesClosed)
        {
            uint pathCost = 0;
            return FindPath(start, goal, Functions.ManhattanDistance, Functions.EuclidianDistanceJPS, out pathCost, out nodesOpen, out nodesClosed);
        }

        public override List<Point> FindPath(Point start, Point goal, out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            return FindPath(start, goal, Functions.ManhattanDistance, Functions.EuclidianDistanceJPS, out pathCost, out nodesOpen, out nodesClosed);
        }
        #endregion

        #region Private methods
        protected override void Initialize()
        {
            base.Initialize();
            parents.SetGrid(unset_point);
        }

        protected override List<Point> ReconstructPath(out uint pathCost)
        {
            int dx, dy;
            var path = new List<Point>();
            var node = goal;
            Point parent;

            pathCost = 0;

            while (node.Equals(start) == false)
            {
                parent = parents[node.X, node.Y];

                pathCost += moveCostFunction(parent.X, parent.Y, node.X, node.Y);

                dx = Math.Sign(parent.X - node.X);
                dy = Math.Sign(parent.Y - node.Y);
                while (node.Equals(parent) == false)
                {
                    path.Add(node);
                    node.X += dx;
                    node.Y += dy;
                }
            }
            path.Add(node);
            return path;
        }

        protected override int WorkSuccessors(int cx, int cy)
        {
	        int nodesOpen = 0;
            int jx, jy;
            var neighbors = GetNeighborhood(cx, cy);
            foreach ( Point neighbor in neighbors )
            {
                var jumpPoint = Jump(neighbor.X, neighbor.Y, cx, cy, neighbor.X - cx, neighbor.Y - cy);
                if (jumpPoint.X != -1 && jumpPoint.Y != -1)
                {
                    jx = jumpPoint.X;
                    jy = jumpPoint.Y;


                    var tentative_g_score = gMap[cx, cy] + moveCostFunction(cx, cy, jx, jy);
                    var current_g_score = gMap[jx, jy];
                    if (inClosedset[jx, jy] == true)
                    {
                        if (tentative_g_score >= current_g_score) continue;
                    }

                    var node_not_in_openset = inOpenset[jx, jy] == false;
                    if (node_not_in_openset || tentative_g_score < current_g_score)
                    {
                        var new_cost = tentative_g_score + heuristicFunction(jx, jy, gx, gy);
                        parents[jx, jy].X = cx;
                        parents[jx, jy].Y = cy;
                        gMap[jx, jy] = tentative_g_score;
                        fMap[jx, jy] = new_cost;
                        if (node_not_in_openset)
                        {
                            openset.Enqueue(new_cost, new Point(jx, jy));
                            inOpenset[jx, jy] = true;
	                        ++nodesOpen;
                        }
                    }
                }
            }
	        return nodesOpen;
        }

        protected virtual List<Point> GetNeighborhood(int cx, int cy)
        {
	        if (_neighborhoodList == null)
		        _neighborhoodList = new List<Point>();
	        else
	        {
				_neighborhoodList.Clear();
			}
            // if has a parent, e.g., is not the starting node, prune the 
            // according to the direction parent -> currentNode
            var p = parents[cx, cy];
            if (p.Equals(unset_point) == false)
            {
                int px = p.X;
                int py = p.Y;
                // get the normalized direction of travel
                var dx = Math.Sign(cx - px);
                var dy = Math.Sign(cy - py);
                //diagonal pruning
                if (dx != 0 && dy != 0)
                {
                    var cmp1 = IsWalkable(cx, cy + dy);
                    var cmp2 = IsWalkable(cx + dx, cy);
                    var cmp3 = IsWalkable(cx - dx, cy);
                    var cmp4 = IsWalkable(cx, cy - dy);

                    if (cmp1) _neighborhoodList.Add(new Point(cx, cy + dy));
                    if (cmp2) _neighborhoodList.Add(new Point(cx + dx, cy));
                    if (cmp1 || cmp2) _neighborhoodList.Add(new Point(cx + dx, cy + dy));
                    if (!cmp3 && cmp1) _neighborhoodList.Add(new Point(cx - dx, cy + dy));
                    if (!cmp4 && cmp2) _neighborhoodList.Add(new Point(cx + dx, cy - dy));
                }
                //horizontal/vertical pruning
                else
                {
                    if (dx == 0)
                    {
                        if (IsWalkable(cx, cy + dy))
                        {
                            _neighborhoodList.Add(new Point(cx, cy + dy));
                            if (!IsWalkable(cx + 1, cy)) _neighborhoodList.Add(new Point(cx + 1, cy + dy));
                            if (!IsWalkable(cx - 1, cy)) _neighborhoodList.Add(new Point(cx - 1, cy + dy));
                        }
                    }
                    else
                    {
                        if (IsWalkable(cx + dx, cy))
                        {
                            _neighborhoodList.Add(new Point(cx + dx, cy));
                            if (!IsWalkable(cx, cy + 1)) _neighborhoodList.Add(new Point(cx + dx, cy + 1));
                            if (!IsWalkable(cx, cy - 1)) _neighborhoodList.Add(new Point(cx + dx, cy - 1));
                        }
                    }
                }
            }
            else
            {
                if (IsWalkable(cx + 0, cy + 1)) _neighborhoodList.Add(new Point(cx + 0, cy + 1));
                if (IsWalkable(cx + 0, cy - 1)) _neighborhoodList.Add(new Point(cx + 0, cy - 1));
                if (IsWalkable(cx + 1, cy + 0)) _neighborhoodList.Add(new Point(cx + 1, cy + 0));
                if (IsWalkable(cx - 1, cy + 0)) _neighborhoodList.Add(new Point(cx - 1, cy + 0));

                if (IsWalkable(cx + 1, cy + 1)) _neighborhoodList.Add(new Point(cx + 1, cy + 1));
                if (IsWalkable(cx + 1, cy - 1)) _neighborhoodList.Add(new Point(cx + 1, cy - 1));
                if (IsWalkable(cx - 1, cy + 1)) _neighborhoodList.Add(new Point(cx - 1, cy + 1));
                if (IsWalkable(cx - 1, cy - 1)) _neighborhoodList.Add(new Point(cx - 1, cy - 1));
            }
            return _neighborhoodList;
        }

        protected virtual Point Jump(int cx, int cy, int px, int py, int dx, int dy)
        {
            //int dx, dy;
            while (true)
            {
                if (!IsWalkable(cx, cy)) return null_point;
                else if (cx == gx && cy == gy) return new Point(cx, cy);

                //dx = cx - px;
                //dy = cy - py;
                // check for forced neighbors along the diagonal
                if (dx != 0 && dy != 0)
                {
                    if (!IsWalkable(cx - dx, cy) && IsWalkable(cx - dx, cy + dy) ||
                        !IsWalkable(cx, cy - dy) && IsWalkable(cx + dx, cy - dy))
                        return new Point(cx, cy);
                }
                // horizontally/vertically
                else
                {
                    if (dx != 0)
                    {
                        if (IsWalkable(cx + dx, cy + 1) && !IsWalkable(cx, cy + 1) ||
                            IsWalkable(cx + dx, cy - 1) && !IsWalkable(cx, cy - 1))
                            return new Point(cx, cy);
                    }
                    else
                    {
                        if (IsWalkable(cx + 1, cy + dy) && !IsWalkable(cx + 1, cy) ||
                            IsWalkable(cx - 1, cy + dy) && !IsWalkable(cx - 1, cy))
                            return new Point(cx, cy);
                    }
                }

                if (dx != 0 && dy != 0)
                {
                    if ( Jump(cx + dx, cy, cx, cy,dx ,0).Equals(null_point) == false ||
                         Jump(cx, cy + dy, cx, cy,0,dy).Equals(null_point) == false)
                        return new Point(cx, cy);
                }

                px = cx;
                py = cy;
                cx = cx + dx;
                cy = cy + dy;
            }
        }
                
        protected bool IsWalkable(int x, int y)
        {
            return (x >= 0 && x < mapWidth) && (y >= 0 && y < mapHeight) && _UnpassableNodes[x, y] == false;
        }
        #endregion
    }
}