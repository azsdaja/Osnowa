namespace Osnowa.Osnowa.Core
{
	using System;

	[Serializable]
	public struct Position : IEquatable<Position>
	{
		public static Position MinValue = new Position(int.MinValue, int.MinValue);

        public static Position Zero { get; } = new Position(0, 0);
		public static Position One { get; } = new Position(1, 1);
		public static Position Up { get; } = new Position(0, 1);
		public static Position Down { get; } = new Position(0, -1);
		public static Position Left { get; } = new Position(-1, 0);
		public static Position Right { get; } = new Position(1, 0);

        public int x { get; }
        public int y { get; }

        public Position(int x, int y)
		{
            this.x = x;
            this.y = y;
		}

		public static bool operator ==(Position lhs, Position rhs)
		{
			if (lhs.x == rhs.x)
				return lhs.y == rhs.y;
			return false;
		}

		public static bool operator !=(Position lhs, Position rhs)
		{
			return !(lhs == rhs);
		}

		public override string ToString()
		{
			return $"({x}, {y})";
		}

		public override bool Equals(object other)
		{
			if (!(other is Position))
				return false;
			return Equals((Position)other);
		}

		public bool Equals(Position other)
		{
			if (x.Equals(other.x))
				return y.Equals(other.y);
			return false;
		}

		public override int GetHashCode()
		{
			int num1 = x;
			int hashCode = num1.GetHashCode();
			num1 = y;
			int num2 = num1.GetHashCode() << 2;
			return hashCode ^ num2;
		}

		public static float Distance(Position first, Position other)
		{
			int num1 = first.x - other.x;
			int num2 = first.y - other.y;
			return (float) Math.Sqrt(num1 * num1 + num2 * num2);
		}

		public int SqrMagnitude => x * x + y * y;

		public static Position From(Position position)
		{
			return new Position(position.x, position.y);
		}

		public static Position operator +(Position first, Position second)
		{
			return new Position(first.x + second.x, first.y + second.y);
		}

		public static Position operator -(Position first, Position second)
		{
			return new Position(first.x - second.x, first.y - second.y);
		}

		public static Position operator *(Position first, Position second)
		{
			return new Position(first.x * second.x, first.y * second.y);
		}

		public static Position operator *(Position first, int multiplier)
		{
			return new Position(first.x * multiplier, first.y * multiplier);
		}
	}
}
