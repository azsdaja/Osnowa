namespace Osnowa.Osnowa.Core
{
	using System;
	using UnityEngine;

	[Serializable]
	public class MatrixGeneric<TValueType>
	{
		[SerializeField]
		protected TValueType[] Values;
		[SerializeField]
		private int _xSize;
		[SerializeField]
		private int _ySize;

		public MatrixGeneric(int xSize, int ySize)
		{
			_xSize = xSize;
			_ySize = ySize;
			Values = new TValueType[xSize * ySize];
		}

		public int XSize => _xSize;

	    public int YSize => _ySize;

	    public TValueType Get(int x, int y)
		{
			return Values[x*YSize + y];
		}

		public TValueType Get(Position position)
		{
			return Values[position.x*YSize + position.y];
		}

		public void Set(int x, int y, TValueType value)
		{
			Values[x*YSize + y] = value;
		}

		public void Set(Position position, TValueType value)
		{
			Values[position.x*YSize + position.y] = value;
		}

		public bool IsWithinBounds(Position position)
		{
			return position.x >= 0 && position.y >= 0
			       && position.x < XSize && position.y < YSize;
		}

		public bool IsWithinBounds(int x, int y)
		{
			return x >= 0 && y >= 0 && x < XSize && y < YSize;
		}
	}
}