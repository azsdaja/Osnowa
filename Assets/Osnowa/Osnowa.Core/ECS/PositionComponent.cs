using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Osnowa.Osnowa.Core.ECS
{
	/// <summary>
	/// Position on the game grid.
	/// </summary>
	[Game, Event(EventTarget.Self), Serializable]
	public class PositionComponent : IComponent
	{
		[SerializeField]
		private Position _position;

		public Position Position
		{
			get { return _position; }
			set { _position = value; }
		}
	}
}