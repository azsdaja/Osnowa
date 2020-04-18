namespace GameLogic.AI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ActionLoop;
	using ActivityCreation;
	using Conditions;
	using Model;
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	public class ActivityResolver : IActivityResolver
	{
		private readonly IUtilityAi _utilityAi;
		private readonly IConditionContext _conditionContext;
		private readonly IActivityCreationContext _activityCreationContext;
		private readonly IActivityInterruptor _activityInterruptor;

		public ActivityResolver(IUtilityAi utilityAi, IConditionContext conditionContext,
			IActivityCreationContext activityCreationContext, IActivityInterruptor activityInterruptor)
		{
			_utilityAi = utilityAi;
			_conditionContext = conditionContext;
			_activityCreationContext = activityCreationContext;
			_activityInterruptor = activityInterruptor;
		}

		public (Skill skillToIntroduceNewActivity, float score) ResolveNewSkillIfApplicable(GameEntity entity, List<Stimulus> stimuli)
		{
			List<Skill> skillsThatMayBreakIn = entity.skills.Skills
				.Where(skill => skill.TriggeringStimuli.Any(triggeringType => 
					stimuli.Select(s => s.Type).Any(actualType => actualType == triggeringType)))
				.Where(s => s.Conditions.All(c => c.Evaluate(entity, _conditionContext))).ToList();

			if (skillsThatMayBreakIn.Count <= 0)
				return (null, 0f);

			StimulusContext stimulusContext = new StimulusContext {TargetEntity = entity};
			float minScoreAllowed = entity.hasActivity ? entity.activity.Activity.Score : 0f;
			(Skill skill, float score) newSkillAndScore = _utilityAi.ResolveSkill(entity, skillsThatMayBreakIn, stimulusContext, minScoreAllowed);

			return newSkillAndScore;
			
/*
			IActivity activityToReturn = null;
			if (newSkillAndScore.skill != null)
			{
				try
				{
					activityToReturn = newSkillAndScore.skill.ActivityCreator.CreateActivity(_activityCreationContext, newSkillAndScore.score, null, entity);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
					activityToReturn = entity.activity.Activity;
				}
			}
			else
			{
				activityToReturn = entity.activity.Activity;
			}

			if (activityToReturn != null)
			{
				_activityInterruptor.FailAndReplace(entity, activityToReturn);
			}
*/
		}
	}
}