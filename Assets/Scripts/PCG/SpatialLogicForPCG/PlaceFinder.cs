namespace PCG.SpatialLogicForPCG
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Osnowa.Osnowa.Core;

	public class PlaceFinder
	{
		public static Bounds GrowRectangleFrom(Position center, Func<Position, bool> isValid, 
			int minSideSize = int.MinValue, int maxSideSize = int.MaxValue, Func<List<Position>, Position> pickGrowthDirection = null)
		{
			var currentBounds = new Bounds(center.x, center.y, 0, 1);
			List<Position> validGrowDirections = new[] {Position.Up, Position.Down, Position.Left, Position.Right}.ToList();

			int currentAreal = 1;
			while (validGrowDirections.Any())
			{
				Position directionToGrow = pickGrowthDirection?.Invoke(validGrowDirections) ?? validGrowDirections.First();

				List<Position> newPositions = GetNewPositions(directionToGrow, currentBounds).ToList();
				currentAreal += newPositions.Count;

				bool canGrow = false;
				if (newPositions.All(isValid))
				{
					Bounds newBoundsCandidate = Bounds.StretchedTo(currentBounds, newPositions.First()); // grow bounds to given direction
					if (newBoundsCandidate.Size.x <= maxSideSize && newBoundsCandidate.Size.y <= maxSideSize)
					{
						canGrow = true;
						currentBounds = newBoundsCandidate;
					}
				}
				if(!canGrow)
				{
					validGrowDirections.Remove(directionToGrow);
				}
			}

			bool satisfiesMinSize = currentBounds.Size.x >= minSideSize && currentBounds.Size.y >= minSideSize;
			return satisfiesMinSize ? currentBounds : Bounds.Zero;
		}

		private static IEnumerable<Position> GetNewPositions(Position directionToGrow, Bounds currentBounds)
		{
			if (directionToGrow.x == 0) // up or down
			{
				int newY = directionToGrow == Position.Up ? currentBounds.Max.y : currentBounds.Min.y - 1;
				for (int newX = currentBounds.Min.x; newX < currentBounds.Max.x; newX++)
				{
					yield return new Position(newX, newY);
				}
			}
			else // left or right
			{
				int newX = directionToGrow == Position.Right ? currentBounds.Max.x : currentBounds.Min.x - 1;
				for (int newY = currentBounds.Min.y; newY < currentBounds.Max.y; newY++)
				{
					yield return new Position(newX, newY);
				}
			}
		}
	}
}