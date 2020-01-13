namespace Osnowa.NaPozniej
{
	using System;
	using UnityEngine;

	[Serializable]
	public class ActorStatus
	{
		[SerializeField] private int _turnsLeft;
		[SerializeField] private readonly ActorStatusDefinition _definition;

		public ActorStatus(ActorStatusDefinition definition, int turnsToStay = -1)
		{
			_definition = definition;
			TurnsLeft = turnsToStay;
		}

		public ActorStatusDefinition Definition => _definition;

		public int TurnsLeft
		{
			get { return _turnsLeft; }
			set { _turnsLeft = value; }
		}
	}
}