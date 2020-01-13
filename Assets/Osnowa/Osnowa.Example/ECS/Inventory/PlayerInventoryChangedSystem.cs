using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Inventory
{
	using GameLogic;

	public class PlayerInventoryChangedSystem : ReactiveSystem<GameEntity>
	{
		private readonly IUiFacade _uiFacade;

		public PlayerInventoryChangedSystem(IContext<GameEntity> context, IUiFacade uiFacade) : base(context)
		{
			_uiFacade = uiFacade;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.PlayerControlled, GameMatcher.Inventory));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isPlayerControlled && entity.hasInventory;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				_uiFacade.RefreshInventory(entity.inventory.EntitiesInInventory);
			}
		}
    }
}