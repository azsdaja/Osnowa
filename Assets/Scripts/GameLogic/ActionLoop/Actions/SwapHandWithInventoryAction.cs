namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Core.ECS;
	using UnityEngine;

	public class SwapHandWithInventoryAction : GameAction
	{
		private readonly int _indexInInventory;
		private readonly LoadViewSystem _loadViewSystem;

		public SwapHandWithInventoryAction(GameEntity entity, int indexInInventory, float energyCost, IActionEffectFactory actionEffectFactory, LoadViewSystem loadViewSystem) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_indexInInventory = indexInInventory;
			_loadViewSystem = loadViewSystem;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			yield return new ControlStaysEffect();

			Guid itemFromHandsId = Entity.entityHolder.EntityHeld;
			GameEntity itemFromHands = Contexts.sharedInstance.game.GetEntityWithId(itemFromHandsId);

			List<Guid> inventoryList = Entity.inventory.EntitiesInInventory;

			Guid itemFromBackpackId = inventoryList[_indexInInventory];
			GameEntity itemFromBackpack = Contexts.sharedInstance.game.GetEntityWithId(itemFromBackpackId);

			inventoryList[_indexInInventory] = Guid.Empty;
			MoveHeldToInventory(inventoryList, _indexInInventory, itemFromHandsId, itemFromHands);
			Entity.ReplaceInventory(inventoryList);
			Entity.ReplaceEntityHolder(itemFromBackpackId);
			itemFromBackpack.ReplacePosition(Entity.position.Position);

			IActionEffect effect = ActionEffectFactory
				.CreateLambdaEffect(viewController => viewController.HoldOnFront(itemFromBackpack), Entity.view.Controller);

			yield return effect;
		}

		private void MoveHeldToInventory(List<Guid> inventoryList, int indexOfItemTakenFromInventory, Guid itemInHandsId, GameEntity itemInHands)
		{
			bool lastIndexIsFree = itemInHands.hasInventoryItem &&
					   inventoryList[itemInHands.inventoryItem.LastIndexInInventory] == Guid.Empty;
			int indexToUse = lastIndexIsFree ? itemInHands.inventoryItem.LastIndexInInventory : indexOfItemTakenFromInventory;
			inventoryList[indexToUse] = itemInHandsId;
			Entity.ReplaceInventory(inventoryList);
			Entity.ReplaceEntityHolder(Guid.Empty);
			itemInHands.ReplaceInventoryItem(indexToUse);
			itemInHands.RemovePosition();
			itemInHands.view.Controller.Transform.position = Vector3.zero;
		}
	}
}