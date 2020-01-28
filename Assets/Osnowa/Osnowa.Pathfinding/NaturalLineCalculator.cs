namespace Osnowa.Osnowa.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Fov;
	using UnityUtilities;

	public class NaturalLineCalculator : INaturalLineCalculator
	{
		private readonly IRasterLineCreator _rasterLineCreator;

		public NaturalLineCalculator(IRasterLineCreator rasterLineCreator)
		{
			_rasterLineCreator = rasterLineCreator;
		}

		public IList<Position> GetFirstLongestNaturalLine(IList<Position> jumpPoints, Func<Position, bool> isWalkable)
		{
			if (jumpPoints.Count == 1)
			{
				return jumpPoints;
			}
			IList<Position> naturalJumpPoints = GetNaturalJumpPoints(jumpPoints);
			if (naturalJumpPoints.Count == 2)
			{
				return _rasterLineCreator.GetRasterLinePermissive(jumpPoints[0].x, jumpPoints[0].y, jumpPoints[1].x, jumpPoints[1].y, 
																	isWalkable, -1, false);
			}
			IList<Position> firstThreeNaturalJumpPoints = naturalJumpPoints.Take(3).ToList();
			Position firstJumpPoint = firstThreeNaturalJumpPoints[0];
			Position rangeCheckBeginning = firstThreeNaturalJumpPoints[1];
			Position rangeCheckEnd = firstThreeNaturalJumpPoints[2];
			IList<Position> rangeToCheck = // note that it's going from range end to range beginning
				_rasterLineCreator.GetRasterLinePermissive(rangeCheckEnd.x, rangeCheckEnd.y, rangeCheckBeginning.x, rangeCheckBeginning.y,
					position => true, -1);
			IList<Position> naturalWay = null;
			foreach (Position checkedPosition in rangeToCheck)
			{
				var xxx
					= _rasterLineCreator.GetRasterLinePermissive(3, 0, 5, 2, isWalkable, -1, false);
					
				IList<Position> bresenhamLineToChecked = 
					_rasterLineCreator.GetRasterLinePermissive(firstJumpPoint.x, firstJumpPoint.y, checkedPosition.x, checkedPosition.y, isWalkable, -1, false);
				bool clearWayToThirdExists = bresenhamLineToChecked.Any() && bresenhamLineToChecked.Last() == checkedPosition;
				if (clearWayToThirdExists)
				{
					naturalWay = bresenhamLineToChecked;
					break;
				}
			}
			return naturalWay;
		}

		public IList<Position> GetFirstLongestNaturalLine(Position startNode, IList<Position> followingJumpPoints, Func<Position, bool> isWalkable)
		{
			return GetFirstLongestNaturalLine(new[] {startNode}.Union(followingJumpPoints).ToList(), isWalkable);
		}

		/// <summary>
		/// Usually the current JPS implementation creates too many jump points (many of them are aligned in one line).
		/// This function gives three first „natural” jump points (two or three), which means they don't form a single line.
		/// </summary>
		public IList<Position> GetNaturalJumpPoints(IList<Position> jumpPoints)
		{
			if (jumpPoints.Count <= 2)
			{
				return jumpPoints;
			}

			var result = new List<Position> {jumpPoints[0]};

			Position currentDirectionNormalized = PositionUtilities.Normalized(jumpPoints[1] - jumpPoints[0]);
			for (int i = 2; i < jumpPoints.Count; i++) // we should start checking from current == third and previous == second
			{
				Position previousPointToCheck = jumpPoints[i - 1];
				Position currentPointToCheck = jumpPoints[i];
				Position currentDirection = PositionUtilities.Normalized(currentPointToCheck - previousPointToCheck);

				if (currentDirection != currentDirectionNormalized) // change of direction implicates that the last jump point was natural
				{
					result.Add(previousPointToCheck);
					currentDirectionNormalized = currentDirection;
				}
			}

			result.Add(jumpPoints.Last());
			return result;
		}
	}
}