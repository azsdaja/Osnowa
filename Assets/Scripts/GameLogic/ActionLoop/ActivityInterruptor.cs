namespace GameLogic.ActionLoop
{
	using Osnowa.Osnowa.AI.Activities;

	internal class ActivityInterruptor : IActivityInterruptor
	{
		public void FailAndReplace(GameEntity entity, IActivity activity, IActivity newActivity)
		{
			activity?.OnFailure(entity);
			ReplaceOrRemove(entity, newActivity);
		}

		private static void ReplaceOrRemove(GameEntity entity, IActivity newActivity)
		{
			if (newActivity != null)
			{
				entity.ReplaceActivity(newActivity);
			}
			else
				entity.RemoveActivity();
		}
	}
}