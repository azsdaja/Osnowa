namespace GameLogic.ActionLoop
{
	using System;
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
		private readonly IActivityInterruptor _activityInterruptor;

		public AiActionResolver(IActionFactory actionFactory, IUtilityAi utilityAi,
			IActivityCreationContext activityCreationContext,
			IActivityInterruptor activityInterruptor)
		{
			_actionFactory = actionFactory;
			_utilityAi = utilityAi;
			_activityCreationContext = activityCreationContext;
			_activityInterruptor = activityInterruptor;
		}

		public IGameAction GetAction(GameEntity entity)
		{
			// todo: remove this and use BlockedUntil. But for sure? Maybe this check is better?
			// now it causes problems when an animating actor is leaving sight (animation doesn't finish -> infinite loop)
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

			if (!entity.hasActivity)
			{
				float score;
				Skill skill = _utilityAi.ResolveSkillWhenIdle(out score, entity);

				Activity newActivity;
				try
				{
					newActivity = skill.ActivityCreator.CreateActivity(_activityCreationContext, score, null, entity);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
					newActivity = new WaitActivity(_actionFactory, 2, "Wait");
				}
				entity.AddActivity(newActivity);
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