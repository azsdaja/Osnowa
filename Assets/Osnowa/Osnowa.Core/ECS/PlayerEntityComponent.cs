using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Osnowa.Osnowa.Core.ECS
{
	[Unique]
	public class PlayerEntityComponent : IComponent
	{
		public Guid Id;
	}
}