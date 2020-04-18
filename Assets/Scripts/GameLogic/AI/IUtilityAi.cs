namespace GameLogic.AI
{
	using System.Collections.Generic;
	using Model;

	public interface IUtilityAi
	{
		(Skill skill, float score) ResolveSkillWhenIdle(GameEntity entity);
		(Skill skill, float score) ResolveSkill(GameEntity entity, IEnumerable<Skill> skillsAplicable, StimulusContext stimulusContext, float minScoreAllowed);
	}
}