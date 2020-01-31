using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Identification
{
	using Unassigned;

	[Serializable]
	public class TeamComponent : IComponent
	{
		public Team Team;
	}
}