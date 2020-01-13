using System;
using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.AI
{
	using GameLogic.AI.Model;

	[Serializable]
	public class SkillsComponent : IComponent
	{
		public List<Skill> Skills;
	}
}