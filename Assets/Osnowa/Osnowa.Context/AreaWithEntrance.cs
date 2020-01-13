namespace Osnowa.Osnowa.Context
{
	using System;
	using System.Collections.Generic;
	using Core;
	using UnityEngine;

	[Serializable]
	public class AreaWithEntrance : Area
	{
		[SerializeField] private Position _entrance;

		public AreaWithEntrance(IEnumerable<Position> positions, Position entrance) : base(positions)
		{
			_entrance = entrance;
		}

		public Position Entrance => _entrance;
	}
}