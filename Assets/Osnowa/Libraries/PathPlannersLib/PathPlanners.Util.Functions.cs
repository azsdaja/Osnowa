namespace Libraries.PathPlannersLib
{
    using System;

    public static class Functions
    {
        public delegate uint Function(int x0, int y0, int x1, int y1);
        public static uint ManhattanDistance(int x0, int y0, int x1, int y1)
        {
            uint dist = 0;
            dist += (x0 > x1 ? Convert.ToUInt32(x0 - x1) : Convert.ToUInt32(x1 - x0));
            dist += (y0 > y1 ? Convert.ToUInt32(y0 - y1) : Convert.ToUInt32(y1 - y0));
            return 1000u * dist;
        }

        public static uint EuclidianDistanceGeneric(int x0, int y0, int x1, int y1)
        {
            return Convert.ToUInt32(Math.Sqrt(Math.Pow(x1 - x0, 2.0) + Math.Pow(y1 - y0, 2.0))*1000);
        }

        public static uint EuclidianDistanceSingle(int x0, int y0, int x1, int y1)
        {
            if (x0 == x1 )
            {                
                if(y0 != y1) return 1000u;
                else return 0;
            }
            else if (y0 == y1) return 1000u;
            else return 1414u;
        }
        
        public static uint EuclidianDistanceJPS(int x0, int y0, int x1, int y1)
        {
            var diffx = Math.Abs(x1 - x0);
            if (diffx == 0)
            {
                if (y0 != y1) return (uint) Math.Abs(y1 - y0) * 1000u;
                else return 0;
            }
            else if (y0 == y1) return (uint) diffx * 1000u;
            //this should never happen according to the nature of the algorithm
            else if (diffx != Math.Abs(y1 - y0)) throw new ArgumentException("This method should not have received this kind of argument when used for Jump Point Search.");
            else return (uint)diffx * 1414u;
        }
    }
    public static class Helper
    {
        public static void SetGrid<T>(this T[,] arr, T value)
        {
            int xx = arr.GetLength(0);
            int yy = arr.GetLength(1);
            for (int x = 0; x < xx; x++)
            {
                for (int y = 0; y < yy; y++)
                {
                    arr[x, y] = value;
                }
            }
        }
    }
}
