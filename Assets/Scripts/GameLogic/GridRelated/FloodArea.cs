namespace GameLogic.GridRelated
{
	using Osnowa;
	using Osnowa.Osnowa.Core;

	public class FloodArea : IFloodArea
	{
        public FloodArea(Position center, int floodRange)
		{
			int boundsSize = floodRange * 2;
			Bounds = new Bounds(center.x - floodRange, center.y - floodRange, boundsSize, boundsSize);
			Center = center;
			ValueMatrix = new int[boundsSize, boundsSize];
		}

		public Bounds Bounds { get; set; }
		public Position Center { get; set; }

		public int ArraySize => ValueMatrix.GetLength(0);

		public Position FurthestPosition { get; set; }

        public int[,] ValueMatrix { get; private set; }

        public int GetValueAtPosition(Position position)
		{
			if (!Bounds.Contains(position))
				return int.MaxValue;
			return ValueMatrix[position.x - Bounds.Min.x, position.y - Bounds.Min.y];
		}

		public void IncreaseMatrix(int expectedMatrixSize)
		{
			ValueMatrix = new int[expectedMatrixSize, expectedMatrixSize];
		}
	}
}