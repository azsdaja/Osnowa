namespace Libraries.SpatialAStar.SpatialAStar.Algorithm
{
	using Osnowa.Osnowa.Core;

	class MyPathNode : IPathNode<Position>
	{
		private bool _isWalkable;

	    public MyPathNode(bool isWalkable, Position position, float cost = 0)
		{
			_isWalkable = isWalkable;
			Position = position;
			Cost = (float) cost;
		}

		public Position Position { get; }

		/// <summary>
		/// 0 is no cost. -1 means it's unwalkable. The higher is the cost, the less optimal is to walk through the position.
		/// </summary>
	    public float Cost { get; set; }

		public bool IsWalkable(Position inContext)
		{
			return _isWalkable;
		}

		public void SetIsWalkable(bool value)
		{
			_isWalkable = value;
		}
	}
}