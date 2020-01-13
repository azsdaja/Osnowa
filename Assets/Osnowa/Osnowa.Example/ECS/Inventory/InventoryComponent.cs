using System;
using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Inventory
{
	public class InventoryComponent : IComponent
	{
		public List<Guid> EntitiesInInventory;
	}
}