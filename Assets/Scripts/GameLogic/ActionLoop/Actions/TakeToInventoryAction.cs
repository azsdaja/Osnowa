namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UnityEngine;

	public class TakeToInventoryAction : GameAction
	{
		public TakeToInventoryAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory) 
			: base(entity, energyCost, actionEffectFactory)
		{
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			yield return new ControlStaysEffect();

			Guid itemToTakeId = Entity.entityHolder.EntityHeld;
			GameEntity itemToTake = Contexts.sharedInstance.game.GetEntityWithId(itemToTakeId);

			List<Guid> inventoryList = Entity.inventory.EntitiesInInventory;
			if (!inventoryList.Any(e => e == Guid.Empty))
			{
				throw new InvalidOperationException("Inventory is full, but entity is trying to pick up something!");
			}

			int chosenIndex;
			bool lastIndexIsFree = itemToTake.hasInventoryItem &&
			                       inventoryList[itemToTake.inventoryItem.LastIndexInInventory] == Guid.Empty;
			chosenIndex = lastIndexIsFree ? itemToTake.inventoryItem.LastIndexInInventory : inventoryList.IndexOf(Guid.Empty);
			inventoryList[chosenIndex] = itemToTakeId;
			Entity.ReplaceInventory(inventoryList);
			Entity.ReplaceEntityHolder(Guid.Empty);
			itemToTake.ReplaceInventoryItem(chosenIndex);
			itemToTake.RemovePosition();
			itemToTake.view.Controller.Transform.position = Vector3.zero;

			yield break;
		}
	}
}