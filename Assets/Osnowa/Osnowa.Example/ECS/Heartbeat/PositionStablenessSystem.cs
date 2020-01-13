using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Heartbeat
{
	/// <summary>
    /// Resolves whether the entity has same position as during last turn.
    /// </summary>
	public class PositionStablenessSystem : ReactiveSystem<GameEntity>
	{
		public PositionStablenessSystem(IContext<GameEntity> context) 
			: base(context)
		{
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.FinishedTurn, GameMatcher.Position, GameMatcher.PositionAfterLastTurn));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isFinishedTurn && entity.hasPositionAfterLastTurn && entity.hasPosition;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				entity.isPositionIsStable = entity.position.Position == entity.positionAfterLastTurn.Position;

				entity.ReplacePositionAfterLastTurn(entity.position.Position);
			}
		}
	}
}