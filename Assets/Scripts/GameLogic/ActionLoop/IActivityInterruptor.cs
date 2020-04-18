namespace GameLogic.ActionLoop
{
	using Osnowa.Osnowa.AI.Activities;

	public interface IActivityInterruptor
	{
		void FailAndReplace(GameEntity entity, IActivity newActivity);
	}
}