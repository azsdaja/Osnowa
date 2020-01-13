namespace GameLogic.ActionLoop.Activities
{
	using System.Collections.Generic;
	using System.Linq;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UnityUtilities;

	public class FollowStepsActivity : Activity
	{
		private readonly IActionFactory _actionFactory;

		private readonly Stack<Position> _stepsToFollow;

		public FollowStepsActivity(IActionFactory actionFactory,
			Stack<Position> stepsToFollow, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_stepsToFollow = stepsToFollow;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			bool isAtDestination = !_stepsToFollow.Any();
			Position nextStep;

			if (isAtDestination)
			{
				return new ActivityStep {State = ActivityState.FinishedSuccess};
			}

			nextStep = _stepsToFollow.Pop();
			Position direction = nextStep - entity.position.Position;
			if (!PositionUtilities.IsOneStep(direction))
			{
				return Fail(entity);
			}

			IGameAction moveGameAction = _actionFactory.CreateJustMoveAction(direction, entity);

			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = moveGameAction
			};
		}
	}
}