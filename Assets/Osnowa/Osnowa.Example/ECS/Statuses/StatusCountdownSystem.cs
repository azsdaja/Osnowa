using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Statuses
{
	public class StatusCountdownSystem : ReactiveSystem<GameEntity>
	{
		public StatusCountdownSystem(IContext<GameEntity> context) : base(context)
		{
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.FinishedTurn);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isFinishedTurn && AllStatusComponents.Conditions(entity);
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entityWithStatus in entities)
			{
				if (entityWithStatus.hasAware)
				{
					int newCounter = entityWithStatus.aware.TurnsLeft - 1;
					if (newCounter <= 0)
						entityWithStatus.RemoveAware();
					else
						entityWithStatus.ReplaceAware(newCounter);
				}

				if (entityWithStatus.hasSleeping)
				{
					int newCounter = entityWithStatus.sleeping.TurnsLeft - 1;
					if (newCounter <= 0)
						entityWithStatus.RemoveSleeping();
					else
						entityWithStatus.ReplaceSleeping(newCounter);
				}

				// and so on for other component types; 
			}
		}
	}
}