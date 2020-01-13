namespace GameLogic.ActionLoop.Activities
{
	using System;
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class PickUpActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly GameEntity _itemToTake;
		private readonly IActivity _goToActivity;

		private NavigationData _navigationData;

		public PickUpActivity(IActionFactory actionFactory, GameEntity itemToTake, INavigator navigator, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_itemToTake = itemToTake;
			_goToActivity = new GoToActivity(actionFactory, itemToTake.position.Position, navigator, "Go to post");
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			if(_itemToTake.held.ParentEntity != Guid.Empty)
			{
				return Fail(entity);
			}

			if (entity.position.Position != _itemToTake.position.Position)
			{
				return _goToActivity.CheckAndResolveStep(entity);
			}

			IGameAction pickUpAction = _actionFactory.CreatePickUpAction(_itemToTake, entity);

			return new ActivityStep
			{
				State = ActivityState.FinishedSuccess,
				GameAction = pickUpAction
			};
		}
	}
}