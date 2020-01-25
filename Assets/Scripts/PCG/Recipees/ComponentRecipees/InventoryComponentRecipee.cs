namespace PCG.Recipees.ComponentRecipees
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Inventory", menuName = "Osnowa/Entities/Recipees/Inventory", order = 0)]
	public class InventoryComponentRecipee : ComponentRecipee
	{
		public int InventoryCapacity = 9;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			var inventory = new List<Guid>();
			for (int i = 0; i < InventoryCapacity; i++)
			{
				inventory.Add(Guid.Empty);
			}
			entity.ReplaceInventory(inventory);
		}
	}
}