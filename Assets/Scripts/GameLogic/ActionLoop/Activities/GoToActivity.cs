namespace GameLogic.ActionLoop.Activities
{
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class GoToActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly Position _destination;
		private readonly INavigator _navigator;

		private NavigationData _navigationData;

		public GoToActivity(IActionFactory actionFactory, Position destination, INavigator navigator, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_destination = destination;
			_navigator = navigator;
		}

		public GoToActivity(IActionFactory actionFactory, NavigationData navigationData, INavigator navigator, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_navigationData = navigationData;
			_destination = navigationData.Destination;
			_navigator = navigator;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			if (_navigationData == null)
			{
				_navigationData = _navigator.GetNavigationData(entity.position.Position, _destination);
				if (_navigationData == null)
				{
					return Fail(entity);
				}
			}

			Position nextStep;
			NavigationResult navigationResult = _navigator.ResolveNextStep(_navigationData, entity.position.Position, out nextStep);

			if (navigationResult == NavigationResult.Finished)
			{
				return Succeed(entity);
			}

			IGameAction moveGameAction = CreateMoveAction(nextStep, entity);

			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = moveGameAction
			};
		}

		private IGameAction CreateMoveAction(Position nextStep, GameEntity entity)
		{
			Position direction = nextStep - entity.position.Position;
			IGameAction moveGameAction = _actionFactory.CreateJustMoveAction(direction, entity);
			return moveGameAction;
		}
	}
}