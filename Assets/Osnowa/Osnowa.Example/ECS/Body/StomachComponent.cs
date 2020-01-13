using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Body
{
	[Serializable]
	public class StomachComponent : IComponent
	{
		public int Satiation;
		public int MaxSatiation;
	}
}