using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Identification
{
	using NaPozniej;

	[Serializable]
	public class TeamComponent : IComponent
	{
		public Team Team;
	}
}