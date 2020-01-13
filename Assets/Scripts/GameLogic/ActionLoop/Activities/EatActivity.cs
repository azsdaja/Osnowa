namespace GameLogic.ActionLoop.Activities
{
	using System;
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class EatActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly GameEntity _itemToEat;

		private NavigationData _navigationData;

		public EatActivity(IActionFactory actionFactory, GameEntity itemToEat, string name) : base(name)
		{
			_actionFactory = actionFactory;
			_itemToEat = itemToEat;
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			bool stillHoldingEatenEntity = entity.hasEntityHolder 
				&& entity.entityHolder.EntityHeld != Guid.Empty // eaten item could have been destroyed, so we don't call _itemToEat.id.Id too eagerly
				&& entity.entityHolder.EntityHeld == _itemToEat.id.Id;
			if (!stillHoldingEatenEntity)
				return Fail(entity);

			bool stillSatiating = entity.hasEdible && entity.edible.Satiety > 0;
			if (!stillSatiating)
			{
				return Succeed(entity);
			}

			IGameAction eatPartAction = _actionFactory.CreateEatItemAction(_itemToEat, entity);

			return new ActivityStep
			{
				State = ActivityState.InProgress,
				GameAction = eatPartAction
			};
		}
	}
}