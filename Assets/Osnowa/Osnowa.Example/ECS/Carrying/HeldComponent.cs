using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Carrying
{
	[Serializable]
	public class HeldComponent : IComponent
	{
		public Guid ParentEntity;
	}
}