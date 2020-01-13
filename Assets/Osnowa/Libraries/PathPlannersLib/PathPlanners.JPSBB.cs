namespace Libraries.PathPlannersLib
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// JPSBB - Variation of JPS that can diagonally move between blocks.
	/// </summary>
	public class JpsTightDiagonal : Jps
    {
	    private List<Point> _neighborhood;
	    public JpsTightDiagonal(bool[,] unpassableNodes) : base(unpassableNodes) { }
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
                    var cmp1 = IsWalkable(cx, cy + dy);
                    var cmp2 = IsWalkable(cx + dx, cy);
                    var cmp3 = IsWalkable(cx - dx, cy);
                    var cmp4 = IsWalkable(cx, cy - dy);

                    if (cmp1) _neighborhood.Add(new Point(cx, cy + dy));
                    if (cmp2) _neighborhood.Add(new Point(cx + dx, cy));
                    if (cmp1 || cmp2) _neighborhood.Add(new Point(cx + dx, cy + dy));
                    //enable cutting corner freely
                    if (!cmp3 && IsWalkable(cx - dx, cy + dy)) _neighborhood.Add(new Point(cx - dx, cy + dy));
                    if (!cmp4 && IsWalkable(cx + dx, cy - dy)) _neighborhood.Add(new Point(cx + dx, cy - dy));
                }
                //horizontal/vertical pruning
                else
                {
                    if (dx == 0)
                    {
                        if (IsWalkable(cx, cy + dy)) _neighborhood.Add(new Point(cx, cy + dy));
                        if (!IsWalkable(cx + 1, cy)) _neighborhood.Add(new Point(cx + 1, cy + dy));
                        if (!IsWalkable(cx - 1, cy)) _neighborhood.Add(new Point(cx - 1, cy + dy));
                    }
                    else
                    {
                        if (IsWalkable(cx + dx, cy)) _neighborhood.Add(new Point(cx + dx, cy));
                        if (!IsWalkable(cx, cy + 1)) _neighborhood.Add(new Point(cx + dx, cy + 1));
                        if (!IsWalkable(cx, cy - 1)) _neighborhood.Add(new Point(cx + dx, cy - 1));
                    }
                }
            }
            else
            {
                if (IsWalkable(cx + 0, cy + 1)) _neighborhood.Add(new Point(cx + 0, cy + 1));
                if (IsWalkable(cx + 0, cy - 1)) _neighborhood.Add(new Point(cx + 0, cy - 1));
                if (IsWalkable(cx + 1, cy + 0)) _neighborhood.Add(new Point(cx + 1, cy + 0));
                if (IsWalkable(cx - 1, cy + 0)) _neighborhood.Add(new Point(cx - 1, cy + 0));

                if (IsWalkable(cx + 1, cy + 1)) _neighborhood.Add(new Point(cx + 1, cy + 1));
                if (IsWalkable(cx + 1, cy - 1)) _neighborhood.Add(new Point(cx + 1, cy - 1));
                if (IsWalkable(cx - 1, cy + 1)) _neighborhood.Add(new Point(cx - 1, cy + 1));
                if (IsWalkable(cx - 1, cy - 1)) _neighborhood.Add(new Point(cx - 1, cy - 1));
            }
            return _neighborhood;
        }
    }
}
