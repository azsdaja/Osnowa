namespace GameLogic.AI.ActivityCreation
{
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	public abstract class ActivityCreator : ScriptableObject
	{
		protected abstract Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, GameEntity entity);

		public Activity CreateActivity(IActivityCreationContext context, float chosenSkillScore, StimulusContext stimulusContext, GameEntity entity)
		{
			Activity activity = CreateActivityInternal(context, stimulusContext, entity);
			activity.Score = chosenSkillScore;
			return activity;
		}
	}
}