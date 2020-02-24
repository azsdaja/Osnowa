namespace GameLogic.AI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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

		public ActivityResolver(IUtilityAi utilityAi, IConditionContext conditionContext,
			IActivityCreationContext activityCreationContext)
		{
			_utilityAi = utilityAi;
			_conditionContext = conditionContext;
			_activityCreationContext = activityCreationContext;
		}

		public IActivity ResolveNewActivityForActorIfApplicable(StimulusType stimulusType, GameEntity targetActor, GameEntity entity)
		{
			IEnumerable<Skill> skillsThatMayBreakIn = entity.skills.Skills
				.Where(s => s.StimuliToBreakIn.Contains(stimulusType))
				.Where(s => s.Conditions.All(c => c.Evaluate(entity, _conditionContext)));

			if (skillsThatMayBreakIn.Any())
			{
				StimulusContext stimulusContext = new StimulusContext {TargetEntity = targetActor};
				float skillScore;
				float minScoreAllowed = entity.hasActivity ? entity.activity.Activity.Score : 0f;
				Skill skill = _utilityAi.ResolveSkill(entity, skillsThatMayBreakIn, stimulusContext, minScoreAllowed, out skillScore);

				IActivity activityToReturn = null;
				if (skill != null)
				{
					try
					{
						activityToReturn = skill.ActivityCreator.CreateActivity(_activityCreationContext, skillScore, null, entity);
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

				return activityToReturn;
			}

			return entity.hasActivity ? entity.activity.Activity : null;
		}
	}
}