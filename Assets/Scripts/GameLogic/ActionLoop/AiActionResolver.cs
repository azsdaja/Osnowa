namespace GameLogic.ActionLoop
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Activities;
	using AI;
	using AI.ActivityCreation;
	using AI.Model;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UnityEngine;

	public class AiActionResolver : IActionResolver
	{
		private readonly IActionFactory _actionFactory;
		private readonly IUtilityAi _utilityAi;
		private readonly IActivityCreationContext _activityCreationContext;
		private readonly IActivityResolver _activityResolver;

		public AiActionResolver(IActionFactory actionFactory, IUtilityAi utilityAi,
			IActivityCreationContext activityCreationContext, IActivityResolver activityResolver)
		{
			_actionFactory = actionFactory;
			_utilityAi = utilityAi;
			_activityCreationContext = activityCreationContext;
			_activityResolver = activityResolver;
		}

		public IGameAction GetAction(GameEntity entity)
		{
			// todo: remove this and use BlockedUntil. But for sure? Maybe this check is better?
			// now it may cause problems when an animating actor is leaving sight (animation doesn't finish -> infinite loop)
			if (entity.view.Controller.Animator.IsAnimating)
			{
				return null;
			}

			if (entity.hasPreparedAction)
			{
				IGameAction actionToReturn = entity.preparedAction.Action;
				entity.RemovePreparedAction();
				return actionToReturn;
			}

			(Skill skill, float score) newSkillAndScore = default;
			if (entity.hasActivity && entity.hasSkills && entity.hasStimuli)
			{
				List<Stimulus> stimuliReceived = entity.stimuli.Stimuli.ToList();
				
				newSkillAndScore = _activityResolver.ResolveNewSkillIfApplicable(entity, stimuliReceived);
			}
			else if (!entity.hasActivity)
			{
				newSkillAndScore = _utilityAi.ResolveSkillWhenIdle(entity);
			}

			if (newSkillAndScore.skill != null)
			{
				Activity newActivity;
				try
				{
					newActivity = newSkillAndScore.skill.ActivityCreator.CreateActivity(_activityCreationContext, newSkillAndScore.score, null, entity);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
					newActivity = new WaitActivity(_actionFactory, 2, "Wait");
				}

				if (entity.hasActivity)
				{
					entity.activity.Activity.OnFailure(entity);
				}
				entity.ReplaceActivity(newActivity);
				newActivity.OnStart(entity);
			}

			IActivity activity = entity.activity.Activity;
			ActivityStep activityStep = activity.CheckAndResolveStep(entity);

			if (activityStep.State == ActivityState.FinishedSuccess || activityStep.State == ActivityState.FinishedFailure)
			{
				entity.RemoveActivity();
			}

			IGameAction actionFromActivity = activityStep.GameAction;

			return actionFromActivity;
		}
	}
}