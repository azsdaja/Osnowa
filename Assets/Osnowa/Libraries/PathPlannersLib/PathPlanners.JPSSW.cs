namespace Libraries.PathPlannersLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>
	/// JPSSW - Variation of JPS with stricter walking, with the goal of not cutting edges.
	/// </summary>
	public class JpsStrictWalk:Jps
    {
	    private List<Point> _neighborhood;
	    public JpsStrictWalk(bool[,] unpassableNodes) : base(unpassableNodes) { }
        protected override List<Point> GetNeighborhood(int cx, int cy)
        {
			if (_neighborhood == null)
				_neighborhood = new List<Point>();
			else
			{
				_neighborhood.Clear();
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
                    if (IsWalkable(cx, cy + dy))
                    {
                        _neighborhood.Add(new Point(cx, cy + dy));
                        if (IsWalkable(cx + dx, cy))
                        {
                            _neighborhood.Add(new Point(cx + dx, cy));
                            _neighborhood.Add(new Point(cx + dx, cy + dy));
                        }
                    }
                    else if (IsWalkable(cx + dx, cy)) _neighborhood.Add(new Point(cx + dx, cy));
                }
                //horizontal/vertical pruning
                else
                {
                    if (dx == 0)
                    {

                        bool walkableAhead = IsWalkable(cx, cy + dy);
                        if (walkableAhead) _neighborhood.Add(new Point(cx, cy + dy));
                        if (!IsWalkable(cx + 1, cy - dy) && IsWalkable(cx + 1, cy))
                        {
                            _neighborhood.Add(new Point(cx + 1, cy));
                            if (walkableAhead) _neighborhood.Add(new Point(cx + 1, cy + dy));
                        }
                        if (!IsWalkable(cx - 1, cy - dy) && IsWalkable(cx - 1, cy))
                        {
                            _neighborhood.Add(new Point(cx - 1, cy));
                            if (walkableAhead) _neighborhood.Add(new Point(cx - 1, cy + dy));
                        }

                    }
                    else
                    {
                        bool walkableAhead = IsWalkable(cx + dx, cy);
                        if (walkableAhead) _neighborhood.Add(new Point(cx + dx, cy));
                        if (!IsWalkable(cx - dx, cy + 1) && IsWalkable(cx, cy + 1))
                        {
                            _neighborhood.Add(new Point(cx, cy + 1));
                            if (walkableAhead) _neighborhood.Add(new Point(cx + dx, cy + 1));
                        }
                        if (!IsWalkable(cx - dx, cy - 1) && IsWalkable(cx, cy - 1))
                        {
                            _neighborhood.Add(new Point(cx, cy - 1));
                            if (walkableAhead) _neighborhood.Add(new Point(cx + dx, cy - 1));
                        }
                    }
                }
            }
            else
            {
                var bpdy = IsWalkable(cx + 0, cy + 1);
                var bmdy = IsWalkable(cx + 0, cy - 1);
                var bpdx = IsWalkable(cx + 1, cy + 0);
                var bmdx = IsWalkable(cx - 1, cy + 0);
                if (bpdy) _neighborhood.Add(new Point(cx + 0, cy + 1));
                if (bmdy) _neighborhood.Add(new Point(cx + 0, cy - 1));
                if (bpdx) _neighborhood.Add(new Point(cx + 1, cy + 0));
                if (bmdx) _neighborhood.Add(new Point(cx - 1, cy + 0));

                if (bpdx && bpdy) _neighborhood.Add(new Point(cx + 1, cy + 1));
                if (bmdx && bpdy) _neighborhood.Add(new Point(cx - 1, cy + 1));
                if (bpdx && bmdy) _neighborhood.Add(new Point(cx + 1, cy - 1));
                if (bmdx && bmdy) _neighborhood.Add(new Point(cx - 1, cy - 1));
            }
            return _neighborhood;
        }
        protected override Point Jump(int cx, int cy, int px, int py, int dx, int dy)
        {
            while (true)
            {
                if (!IsWalkable(cx, cy)) return null_point;
                else if (cx == gx && cy == gy) return new Point(cx, cy);

                //dx = cx - px;
                //dy = cy - py;
                // check for forced neighbors along the diagonal
                if (dx != 0 && dy != 0)
                {
                    if (IsWalkable(cx + dx, cy + dy) && !IsWalkable(cx + dx, cy))
                        return new Point(cx, cy);
                }
                // horizontally/vertically
                else
                {
                    if (dx != 0)
                    {
                        if (IsWalkable(cx, cy + 1) && !IsWalkable(cx - dx, cy + 1) ||
                            IsWalkable(cx, cy - 1) && !IsWalkable(cx - dx, cy - 1))
                            return new Point(cx, cy);
                    }
                    else
                    {
                        if (IsWalkable(cx + 1, cy) && !IsWalkable(cx + 1, cy - dy) ||
                            IsWalkable(cx - 1, cy) && !IsWalkable(cx - 1, cy - dy))
                            return new Point(cx, cy);
                    }
                }

                if (dx != 0 && dy != 0)
                {
                    if (Jump(cx + dx, cy, cx, cy, dx, 0).Equals(null_point) == false ||
                         Jump(cx, cy + dy, cx, cy, 0, dy).Equals(null_point) == false)
                        return new Point(cx, cy);
                }

                px = cx;
                py = cy;
                cx = cx + dx;
                cy = cy + dy;
            }
        }

        //protected override Point Jump__ORIGINAL(int cx, int cy, int px, int py, int dx, int dy)
        //{
        //    while (true)
        //    {
        //        if (!IsWalkable(cx, cy)) return null_point;
        //        else if (cx == gx && cy == gy) return new Point(cx, cy);

        //        //dx = cx - px;
        //        //dy = cy - py;
        //        // check for forced neighbors along the diagonal
        //        if (dx != 0 && dy != 0)
        //        {
        //            if (IsWalkable(cx + dx, cy + dy) && !IsWalkable(cx + dx, cy))
        //                return new Point(cx, cy);
        //        }
        //        // horizontally/vertically
        //        else
        //        {
        //            if (dx != 0)
        //            {
        //                if (IsWalkable(cx, cy + 1) && !IsWalkable(cx - dx, cy + 1) ||
        //                    IsWalkable(cx, cy - 1) && !IsWalkable(cx - dx, cy - 1))
        //                    return new Point(cx, cy);
        //            }
        //            else
        //            {
        //                if (IsWalkable(cx + 1, cy) && !IsWalkable(cx + 1, cy - dy) ||
        //                    IsWalkable(cx - 1, cy) && !IsWalkable(cx - 1, cy - dy))
        //                    return new Point(cx, cy);
        //            }
        //        }

        //        if (dx != 0 && dy != 0)
        //        {
        //            if (Jump(cx + dx, cy, cx, cy, dx, 0).Equals(null_point) == false ||
        //                 Jump(cx, cy + dy, cx, cy, 0, dy).Equals(null_point) == false)
        //                return new Point(cx, cy);
        //        }

        //        px = cx;
        //        py = cy;
        //        cx = cx + dx;
        //        cy = cy + dy;
        //    }
        //}
    }
}
