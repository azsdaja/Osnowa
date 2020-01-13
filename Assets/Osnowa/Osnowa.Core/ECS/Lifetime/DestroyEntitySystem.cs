using System;
using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Lifetime
{
	public class DestroyEntitySystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext _context;

		public DestroyEntitySystem(GameContext context) : base(context)
		{
			_context = context;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.MarkedForDestruction);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isMarkedForDestruction;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				if (entity.hasHeld)
				{
					Guid parentId = entity.held.ParentEntity;
					if (parentId != Guid.Empty)
					{
						GameEntity parent = _context.GetEntityWithId(parentId);
						parent.ReplaceEntityHolder(Guid.Empty);
					}
				}

				if (entity.hasView)
				{
					entity.view.Controller.Free();
				}

				Guid idToDestroy = entity.id.Id;
				entity.Destroy();
			}
		}
	}
}