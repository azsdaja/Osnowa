namespace Osnowa.Osnowa.FOV
{
	using System;
	using System.Collections.Generic;
	using Core;
	using UnityEngine;

	public class BresenhamLineCreator : IRasterLineCreator
	{
		public IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2)
		{
			return GetRasterLine(currentX, y1, x2, y2, -1, position => true);
		}

		public IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, int maxLength)
		{
			return GetRasterLine(currentX, y1, x2, y2, maxLength, position => true);
		}

		public IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, Func<Position, bool> isPassing,
			bool includeBlockingPoint = true)
		{
			return GetRasterLine(currentX, y1, x2, y2, -1, isPassing, includeBlockingPoint);
		}

		public IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, Func<Position, bool> isPassing,
			int maxLength = -1, bool includeBlockingPoint = true)
		{
			return GetRasterLine(currentX, y1, x2, y2, maxLength, isPassing, includeBlockingPoint);
		}

		/// <summary>
		/// Creates a raster line made of points connecting two points on a grid. Stops on blocking cells.
		/// </summary>
		/// <param name="maxLength">Limit for length of line. If set to -1, unlimited.</param>
		/// <param name="includeBlockingPoint">Indicates if the blocking point should be included in the result or not.</param>
		/// <example>
		/// .......**2
		/// ...****...
		/// 1**.......
		/// </example>
		/// <remarks>Anno Domini 1965.</remarks>
		public IList<Position> GetRasterLine(int x1, int y1, int x2, int y2,
			int maxLength, Func<Position, bool> isPassing, bool includeBlockingPoint = true)
		{
			var result = new List<Position>();

			bool lengthLimited;
			int currentX;
			int currentY;
			int dxWhenStable;
			int dyWhenStable;
			int dxWhenAdvance;
			int dyWhenAdvance;
			int shorterAxisLength;
			int longerAxisLength;
			int index;
			int absXLength;
			int absYLength;
			PrecalculateBresenhamParameters(out lengthLimited, x1, y1, x2, y2, maxLength, out currentX, out currentY,
				out dxWhenStable,
				out dyWhenStable, out dxWhenAdvance, out dyWhenAdvance, out shorterAxisLength, out longerAxisLength, out index, out absXLength, out absYLength);
			for (int i = 0; i <= longerAxisLength; i++)
			{
				var currentPosition = new Position(currentX, currentY);
				if (lengthLimited && i + 1 > maxLength) break;
				result.Add(currentPosition);
				if (!isPassing(currentPosition))
				{
					if (!includeBlockingPoint) result.Remove(currentPosition);
					break;
				}
				index += shorterAxisLength;
				if (index < longerAxisLength)
				{
					currentX += dxWhenAdvance;
					currentY += dyWhenAdvance;
				}
				else
				{
					currentX += dxWhenStable;
					currentY += dyWhenStable;
					index -= longerAxisLength;
				}
			}
			return result;
		}

		/// <summary>
		/// Works like GetRasterLine, but chooses secondary options for next move if the primary option is blocked
		/// <example>
		/// ....*****2
		/// .***####..
		/// 1##.......
		/// </example>
		/// </summary>
		public IList<Position> GetRasterLinePermissive(int x1, int y1, int x2, int y2, Func<Position, bool> isPassing, int maxLength, 
			bool includeBlockingPoint = true)
		{
			var result = new List<Position> {new Position(x1, y1)};

			bool lengthLimited;
			int currentX;
			int currentY;
			int dxWhenStable;
			int dyWhenStable;
			int dxWhenAdvance;
			int dyWhenAdvance;
			int shorterAxisLength;
			int longerAxisLength;
			int index;
			int absXLength;
			int absYLength;
			PrecalculateBresenhamParameters(out lengthLimited, x1, y1, x2, y2, maxLength, out currentX, out currentY,
				out dxWhenStable,
				out dyWhenStable, out dxWhenAdvance, out dyWhenAdvance, out shorterAxisLength, out longerAxisLength, out index, out absXLength, out absYLength);
			bool xIsLongerAxis = longerAxisLength == absXLength;
			for (int i = 0; i < longerAxisLength; i++)
			{
				if (lengthLimited && i + 1 > maxLength) break;

				index += shorterAxisLength;

				int primaryX, primaryY, secondaryX, secondaryY;
				bool advancingIsPrimary = index < longerAxisLength;
				if (advancingIsPrimary)
				{
					primaryX = currentX + dxWhenAdvance;
					primaryY = currentY + dyWhenAdvance;
					secondaryX = currentX + dxWhenStable;
					secondaryY = currentY + dyWhenStable;
				}
				else
				{
					primaryX = currentX + dxWhenStable;
					primaryY = currentY + dyWhenStable;
					secondaryX = currentX + dxWhenAdvance;
					secondaryY = currentY + dyWhenAdvance;
				}

				var primaryCandidate = new Position(primaryX, primaryY);
				if (isPassing(primaryCandidate))
				{
					currentX = primaryX;
					currentY = primaryY;
					result.Add(primaryCandidate);
					if(!advancingIsPrimary)
						index -= longerAxisLength;
					continue;
				}

				var secondaryCandidate = new Position(secondaryX, secondaryY);
				bool xToTargetIsLonger = Math.Abs(x2 - secondaryX) >= Math.Abs(y2 - secondaryY);
				bool longerAxisInTotalIsLongerAxisInRemainder = xToTargetIsLonger == xIsLongerAxis;
				if (longerAxisInTotalIsLongerAxisInRemainder && isPassing(secondaryCandidate))
				{
					currentX = secondaryX;
					currentY = secondaryY;
					result.Add(secondaryCandidate);
					if (advancingIsPrimary)
					{
						index -= longerAxisLength;
					}
				}
				else if (includeBlockingPoint)
				{
					result.Add(primaryCandidate);
					break;
				}
			}
			return result;
		}

		private void PrecalculateBresenhamParameters(out bool lengthLimited, int x1, int y1, int x2, int y2, int maxLength, 
			out int currentX, out int currentY, out int dxWhenStable, out int dyWhenStable, out int dxWhenAdvance, out int dyWhenAdvance, 
			out int shorterAxisLength, out int longerAxisLength, out int index, out int absXLength, out int absYLength)
		{
			lengthLimited = maxLength != -1;
			int xLength = x2 - x1;
			int yLength = y2 - y1;
			currentX = x1;
			currentY = y1;
			dxWhenStable = 0;
			dyWhenStable = 0;
			dxWhenAdvance = 0;
			dyWhenAdvance = 0;
			if (xLength < 0) dxWhenStable = -1;
			else if (xLength > 0) dxWhenStable = 1;
			if (yLength < 0) dyWhenStable = -1;
			else if (yLength > 0) dyWhenStable = 1;
			absXLength = Mathf.Abs(xLength);
			absYLength = Mathf.Abs(yLength);
			if (absYLength > absXLength)
			{
				shorterAxisLength = absXLength;
				longerAxisLength = absYLength;
				dxWhenAdvance = 0;
				if (yLength < 0) dyWhenAdvance = -1;
				else if (yLength > 0) dyWhenAdvance = 1;
			}
			else
			{
				shorterAxisLength = absYLength;
				longerAxisLength = absXLength;
				dyWhenAdvance = 0;
				if (xLength < 0) dxWhenAdvance = -1;
				else if (xLength > 0) dxWhenAdvance = 1;
			}
			index = longerAxisLength/2;
		}
	}
}