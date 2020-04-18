namespace GameLogic.AI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ActivityCreation;
	using Conditions;
	using Model;
	using SkillEvaluation;

	public class UtilityAi : IUtilityAi
	{
		private readonly IActivityCreationContext _activityCreationContext;
		private readonly ISkillEvaluationContext _skillEvaluationContext;
		private readonly IConditionContext _conditionContext;

		public UtilityAi(IActivityCreationContext activityCreationContext,  ISkillEvaluationContext skillEvaluationContext, 
			IConditionContext conditionContext)
		{
			_activityCreationContext = activityCreationContext;
			_skillEvaluationContext = skillEvaluationContext;
			_conditionContext = conditionContext;
		}
		
		public (Skill skill, float score) ResolveSkillWhenIdle(GameEntity entity)
		{
			IEnumerable<Skill> skillsApplicable = entity.skills.Skills
				.Where(s => s.Conditions.All(c => c.Evaluate(entity, _conditionContext)));
			return ResolveSkill(entity, skillsApplicable, null, 0f);
		}

		public (Skill skill, float score) ResolveSkill(GameEntity entity, IEnumerable<Skill> skillsAplicable, StimulusContext stimulusContext, float minScoreAllowed)
		{
			var skillsToScores = new List<Tuple<Skill, float>>();
			foreach (Skill actorSkill in skillsAplicable)
			{
				float score = actorSkill.Evaluator.EvaluateSkill(_skillEvaluationContext, actorSkill, entity);
				skillsToScores.Add(Tuple.Create(actorSkill, score));
			}

			skillsToScores.Sort((skillToScore1, skillToScore2) => skillToScore1.Item2.CompareTo(skillToScore2.Item2));

			Skill bestSkill = skillsToScores.Last().Item1;
			float bestSkillScore = skillsToScores.Last().Item2;

			return minScoreAllowed > bestSkillScore ? default : (bestSkill, bestSkillScore);
		}
	}
}