using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Body
{
	[Serializable]
	public class SexComponent : IComponent
	{
		public bool IsMale;
	}
}