namespace GameLogic.GridRelated
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;

	public class FovArea
	{
		public HashSet<Position> Positions { get; set; }
		public Position Center { get; set; }
		public int SightRange { get; set; }
	}
}