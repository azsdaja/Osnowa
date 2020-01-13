namespace Osnowa.Osnowa.AI.Activities
{
	using Core.ActionLoop;

	public class ActivityStep
	{
		public IGameAction GameAction { get; set; }
		public ActivityState State { get; set; }
	}
}