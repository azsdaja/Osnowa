namespace GameLogic.ActionLoop
{
	using Osnowa.Osnowa.AI.Activities;

	internal class ActivityInterruptor : IActivityInterruptor
	{
		public void FailAndReplace(GameEntity entity, IActivity newActivity)
		{
			if (entity.hasActivity)
			{
				entity.activity.Activity.OnFailure(entity);	
			}
			
			if (newActivity == null)
			{
				if(entity.hasActivity)
					entity.RemoveActivity();
			}
			else
				entity.ReplaceActivity(newActivity);
		}
	}
}