namespace GameLogic.AI.ActivityCreation
{
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	[CreateAssetMenu(fileName = "SleepAnywhereActivityCreator", menuName = "Osnowa/AI/Activities/SleepAnywhereActivityCreator", order = 0)]
	public class SleepAnywhereActivityCreator : ActivityCreator
	{
		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, GameEntity entity)
		{
			entity.ReplaceSleeping(30);
			var sleepActivity = new WaitActivity(context.ActionFactory, 30, "Sleep", entityArg => entity.hasSleeping);
			return sleepActivity;
		}
	}
}