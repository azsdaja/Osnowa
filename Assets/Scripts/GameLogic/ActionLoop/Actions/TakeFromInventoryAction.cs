namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Core.ECS;

	public class TakeFromInventoryAction : GameAction
	{
		private readonly int _indexInInventory;
		private readonly LoadViewSystem _loadViewSystem;

	    public TakeFromInventoryAction(GameEntity entity, int indexInInventory, float energyCost, IActionEffectFactory actionEffectFactory, LoadViewSystem loadViewSystem)
			: base(entity, energyCost, actionEffectFactory)
		{
			_indexInInventory = indexInInventory;
			_loadViewSystem = loadViewSystem;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			yield return new ControlStaysEffect();

			List<Guid> inventoryList = Entity.inventory.EntitiesInInventory;

			Guid itemToTakeId = inventoryList[_indexInInventory];
			GameEntity itemToTake = Contexts.sharedInstance.game.GetEntityWithId(itemToTakeId);

			inventoryList[_indexInInventory] = Guid.Empty;
			Entity.ReplaceInventory(inventoryList);
			Entity.ReplaceEntityHolder(itemToTakeId);
			itemToTake.ReplacePosition(Entity.position.Position);

			IActionEffect effect = ActionEffectFactory
				.CreateLambdaEffect(viewController => viewController.HoldOnFront(itemToTake), Entity.view.Controller);

			yield return effect;
		}
	}
}