namespace PCG.Recipees.ComponentRecipees
{
	using System.Collections.Generic;
	using System.Linq;
	using GameLogic.AI.Model;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Skills", menuName = "Osnowa/Entities/Recipees/Skills", order = 0)]
	public class SkillsComponentRecipee : ComponentRecipee
	{
		public List<Skill> Skills;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			if(!entity.hasSkills)
				entity.ReplaceSkills(Skills);
			else
			{
				List<Skill> derivedSkills = entity.skills.Skills;
				List<Skill> allSkills = derivedSkills.Union(Skills).ToList();
				entity.ReplaceSkills(allSkills);
			}
		}
	}
}