namespace GameLogic.AI
{
	using System.Collections.Generic;
	using Model;

	public interface IUtilityAi
	{
		Skill ResolveSkillWhenIdle(out float skillScore, GameEntity entity);
		Skill ResolveSkill(GameEntity entity, IEnumerable<Skill> skillsAplicable, StimulusContext stimulusContext, float minScoreAllowed, 
			out float skillScore);
	}
}