using System;
using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Heartbeat
{
	/// <summary>
    /// Updates position of entities held by given entity.
    /// </summary>
	public class MoveHeldAlongSystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext _context;

		public MoveHeldAlongSystem(GameContext context) 
			: base(context)
		{
			_context = context;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.FinishedTurn, GameMatcher.Position));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isFinishedTurn && entity.hasPosition;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				if (entity.hasEntityHolder && entity.entityHolder.EntityHeld != Guid.Empty)
				{
					GameEntity entityHeld = _context.GetEntityWithId(entity.entityHolder.EntityHeld);
					if (entityHeld.hasPosition)
					{
						entityHeld.ReplacePosition(entity.position.Position);
					}
				}
			}
		}
	}
}