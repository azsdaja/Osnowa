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
		public Position Position;
	}
}