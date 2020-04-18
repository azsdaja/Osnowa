namespace GameLogic.AI
{
	using System.Collections.Generic;
	using Model;
	using Osnowa.Osnowa.AI.Activities;

	public interface IActivityResolver
	{
		(Skill skillToIntroduceNewActivity, float score) ResolveNewSkillIfApplicable(GameEntity entity, List<Stimulus> stimuli);
	}
}