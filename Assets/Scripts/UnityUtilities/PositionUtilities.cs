namespace UnityUtilities
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public static class PositionUtilities
	{
		public static Position Min => new Position(-int.MaxValue, -int.MaxValue);

		public static Position SnapToXAxisNormalized(Position vector)
		{
			return new Position(Sign(vector.x), 0);
		}

		public static Position SnapToYAxisNormalized(Position vector)
		{
			return new Position(0, Sign(vector.y));
		}

		public static Position Normalized(Position vector)
		{
			Vector2 normalizedNonDiscrete = new Vector2(vector.x, vector.y).normalized;
			return new Position(Mathf.RoundToInt(normalizedNonDiscrete.x), Mathf.RoundToInt(normalizedNonDiscrete.y));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Position Average(IList<Position> vectors)
		{
			var sum = new Position();
			foreach (var vector in vectors)
			{
				sum += vector;
			}
			var average = new Position(sum.x / vectors.Count, sum.y / vectors.Count);
			return average;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int WalkDistance(Position vector1, Position vector2)
		{
			int xDistance = Math.Abs(vector1.x - vector2.x);
			int yDistance = Math.Abs(vector1.y - vector2.y);

			return xDistance > yDistance ? xDistance : yDistance;
		}

		public static bool IsOneStep(Position vector)
		{
			if (vector.x == 0 && vector.y == 0)
				return false;
			return vector.x >= -1 && vector.x <= 1 
				&& vector.y >= -1 && vector.y <= 1 && 
				 !(vector == Position.Zero);
		}
	
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<Position> Neighbours4List(Position vector)
		{
			return new List<Position>(4)
			{
				// this order should provide optimal performance when accessing arrays
				vector + Position.Up,
				vector + Position.Down,
				vector + Position.Left,
				vector + Position.Right,
			};
		}
	
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<Position> Neighbours4(Position vector)
		{
			// this order should provide optimal performance when accessing arrays
			yield return vector + Position.Up;
			yield return vector + Position.Down;
			yield return vector + Position.Left;
			yield return vector + Position.Right;
		}
	
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<Position> Neighbours4Diagonal(Position vector)
		{
			yield return vector + new Position(-1, -1);
			yield return vector + new Position(-1, 1);
			yield return vector + new Position(1, -1);
			yield return vector + new Position(1, 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<Position> Neighbours8(Position vector)
		{
			return new List<Position>(8)
			{
				// this order should provide optimal performance when accessing arrays
				vector + Position.Up,
				vector + Position.Down,
				vector + new Position(-1, 1),
				vector + new Position(-1, -1),
				vector + new Position(1, 1),
				vector + new Position(1, -1),
				vector + Position.Left,
				vector + Position.Right,
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Position MultiplyAndRound(Position position, float factor)
		{
			return new Position((int) Math.Round(position.x * factor), (int) Math.Round(position.y * factor));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<Position> Neighbours8Enumerable(Position vector)
		{
			yield return vector + Position.Up;
			yield return vector + Position.Down;
			yield return vector + Position.Up + Position.Left;
			yield return vector + Position.Down + Position.Left;
			yield return vector + Position.Up + Position.Right;
			yield return vector + Position.Left;
			yield return vector + Position.Down + Position.Right;
			yield return vector + Position.Right;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<Position> MeAndNeighbours8(Position position)
		{
			yield return position;
			foreach (var neighbour in Neighbours8(position))
			{
				yield return neighbour;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(int number)
	    {
	        return number > 0 ? 1 : number == 0 ? 0 : -1;
	    }
	}
}