namespace GameLogic.ActionLoop.Activities
{
	using System;
	using Osnowa.Osnowa.AI.Activities;

	public class WaitActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private int _turnsLeftToWait;

		public WaitActivity(IActionFactory actionFactory, int turnsLeftToWait, string name, Predicate<GameEntity> isStillValid = null) 
			: base(name, isStillValid)
		{
			_actionFactory = actionFactory;
			_turnsLeftToWait = turnsLeftToWait;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			if (_turnsLeftToWait > 0)
			{
				--_turnsLeftToWait;
				return new ActivityStep
				{
					GameAction = _actionFactory.CreatePassAction(entity),
					State = ActivityState.InProgress
				};
			}
			return Succeed(entity);
		}
	}
}