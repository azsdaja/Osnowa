namespace PCG
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	/// <summary>
	/// A matrix used for for generating map ingredients (layers). Can be used for presentation and for being input for other map ingredient generators.
	/// </summary>
	[Serializable]
	public class ValueMap
	{
		[SerializeField]
		private readonly int _positionsPerCell;
		[SerializeField]
		private readonly int _sizeX;
		private readonly int _xCells;
		[SerializeField]
		private readonly int _sizeY;
		private readonly int _yCells;
		private readonly float[,] _values;

		public int PositionsPerCell
		{
			get { return _positionsPerCell; }
		}

		public int XSize
		{
			get { return _sizeX; }
		}

		public int YSize
		{
			get { return _sizeY; }
		}

		public ValueMap(int positionsPerCell, int sizeX, int sizeY)
		{
			//if(positionsPerCell % 2 == 0)
			//	throw new ArgumentException("positionsPerCell should be odd.");
			if(sizeX % positionsPerCell > 0 || sizeY % positionsPerCell > 0)
				throw new ArgumentException("sizeX or sizeY not matching the given positionsPerCell: " + sizeX + ", " + sizeY);

			_positionsPerCell = positionsPerCell;
			_sizeX = sizeX;
			_sizeY = sizeY;
			_xCells = sizeX/positionsPerCell;
			_yCells = sizeY/positionsPerCell;
			_values = new float[_xCells,_yCells];
		}

		public float Get(Position position)
		{
			int xInMap = position.x/PositionsPerCell;
			int yInMap = position.y/PositionsPerCell;
			return _values[xInMap, yInMap];
		}

		public float Get(int x, int y)
		{
			int xInMap = x/PositionsPerCell;
			int yInMap = y/PositionsPerCell;
			return _values[xInMap, yInMap];
		}

		public float GetPure(Position position)
		{
			int xInMap = position.x;
			int yInMap = position.y;
			return _values[xInMap, yInMap];
		}

		public void Set(Position position, float value)
		{
			int xInMap = position.x / PositionsPerCell;
			int yInMap = position.y / PositionsPerCell;
			_values[xInMap, yInMap] = value;
		}

		public void Set(int x, int y, float value)
		{
			int xInMap = x / PositionsPerCell;
			int yInMap = y / PositionsPerCell;
			_values[xInMap, yInMap] = value;
		}

		public IEnumerable<Position> AllCellMiddles()
		{
			int fromCornerToMiddle = _positionsPerCell == 1 ? 1 : _positionsPerCell / 2;

			for (int cellMiddleX = fromCornerToMiddle -1; cellMiddleX < XSize; cellMiddleX += fromCornerToMiddle)
			{
				for (int cellMiddleY = fromCornerToMiddle - 1; cellMiddleY < YSize; cellMiddleY += fromCornerToMiddle)
				{
					yield return new Position(cellMiddleX, cellMiddleY);
				}
			}
		}

		public static IEnumerable<Position> AllCellMiddles(int positionsPerCell, int xSize, int ySize)
		{
			int fromCornerToMiddle = positionsPerCell == 1 ? 1 : positionsPerCell / 2;

			for (int cellMiddleX = fromCornerToMiddle; cellMiddleX < xSize; cellMiddleX += fromCornerToMiddle)
			{
				for (int cellMiddleY = fromCornerToMiddle; cellMiddleY < ySize; cellMiddleY += fromCornerToMiddle)
				{
					yield return new Position(cellMiddleX, cellMiddleY);
				}
			}
		}

		public bool IsWithinBounds(Position position)
		{
			return position.x >= 0 && position.y >= 0
				   && position.x < XSize && position.y < YSize;
		}

		public void Clear()
		{
			Array.Clear(_values, 0, _values.Length);
		}
	}
}