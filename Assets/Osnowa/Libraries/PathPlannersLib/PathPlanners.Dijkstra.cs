namespace Libraries.PathPlannersLib
{
    using System.Collections.Generic;

    public class Dijkstra : AStar
    {
        public Dijkstra(bool[,] unpassableNodes) : base(unpassableNodes) { }

        public override List<Point> FindPath(Point start, Point goal, out int nodesOpen, out int nodesClosed)
        {
            uint pathCost = 0;
            return FindPath(start, goal, (x0, y0, x1, y1) => 0 , Functions.EuclidianDistanceSingle, out pathCost, out nodesOpen, out nodesClosed);
        }

        public override List<Point> FindPath(Point start, Point goal, out uint pathCost, out int nodesOpen, out int nodesClosed)
        {
            return FindPath(start, goal, (x0, y0, x1, y1) => 0, Functions.EuclidianDistanceSingle,  out pathCost, out nodesOpen, out nodesClosed);
        }
    }
}
