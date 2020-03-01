using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Heartbeat
{
	public class PreTurnSystem : ReactiveSystem<GameEntity>
	{
		public PreTurnSystem(IContext<GameEntity> context) 
			: base(context)
		{
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.ExecutePreTurn));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isExecutePreTurn;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				// empty logic for now
				
				entity.isExecutePreTurn = false;
			}
		}
	}
}