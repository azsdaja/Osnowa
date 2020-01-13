using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Body
{
	[Serializable]
	public class IntegrityComponent : IComponent
	{
		public float Integrity;
		public float MaxIntegrity;
	}
}