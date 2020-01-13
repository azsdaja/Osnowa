namespace Osnowa.Osnowa.AI.Activities
{
	using Core.ActionLoop;

	public class SingleActionActivity : Activity
	{
		private readonly IGameAction _specificActorAction;
		
		public SingleActionActivity(IGameAction action, string name) : base(name)
		{
			_specificActorAction = action;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			return Succeed(entity, _specificActorAction);
		}
	}
}