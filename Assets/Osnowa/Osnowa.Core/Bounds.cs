namespace Osnowa.Osnowa.Core
{
    using System;
    using System.Collections.Generic;

    public struct Bounds : IEquatable<Bounds>
    {
        public Bounds(int xMin, int yMin, int sizeX, int sizeY)
        {
            Min = new Position(xMin, yMin);
            Size = new Position(sizeX, sizeY);
        }

        public Bounds(Position minPosition, Position size)
        {
            Min = minPosition;
            Size = size;
        }

        public Position Center => new Position(Min.x + Size.x / 2, Min.y + Size.y / 2);

        public Position Max => Min + Size + new Position(-1, -1);

        public Position Min { get; }

        public Position Size { get; }

        public bool Contains(Position position)
        {
            return position.x >= Min.x
                   && position.y >= Min.y
                   && position.x <= Min.x + Size.x
                   && position.y <= Min.y + Size.y;
        }

        /// <summary>
        /// Returns source Bounds which are stretched so that they contain consideredPosition.
        /// </summary>
        public static Bounds StretchedTo(Bounds source, Position consideredPosition)
        {
            int xMin = source.Min.x;
            int yMin = source.Min.y;
            int xMax = source.Max.x;
            int yMax = source.Max.y;

            if (consideredPosition.x < xMin)
            {
                xMin = consideredPosition.x;
            }

            if (consideredPosition.y < yMin)
            {
                yMin = consideredPosition.y;
            }

            if (consideredPosition.x >= xMax)
            {
                xMax = consideredPosition.x + 1;
            }

            if (consideredPosition.y >= yMax)
            {
                yMax = consideredPosition.y + 1;
            }

            return new Bounds(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
        }

        public static Bounds Zero => new Bounds(0, 0, 0, 0);
        
        public IEnumerable<Position> AllPositions()
        {
            for (int x = Min.x; x <= Max.x; x++)
            {
                for (int y = Min.y; y <= Max.y; y++)
                {
                    yield return new Position(x, y);
                }
            }
        }

        public static Bounds CenteredOn(Position center, int range)
        {
            int size = 2 * range + 1;
            Position boundsMin = new Position(center.x - range, center.y - range);
            Position boundsSize = new Position(size, size);
            return new Bounds(boundsMin, boundsSize);
        }

        public override bool Equals(object other)
        {
            return other is Bounds bounds && this.Equals(bounds);
        }

        public bool Equals(Bounds other)
        {
            return Min.Equals(other.Min) && Size.Equals(other.Size);
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Size.GetHashCode() << 2;
        }
    }
}