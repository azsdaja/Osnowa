using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Initiative
{
	using ActionLoop;

	public class GiveControlSystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext _context;
		private readonly IEntityController _entityController;

		public GiveControlSystem(GameContext context, IEntityController entityController) : base(context)
		{
			_context = context;
			_entityController = entityController;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Energy, GameMatcher.EnergyReady));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasEnergy && entity.isEnergyReady;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				bool controlPassed = _entityController.GiveControl(entity);
                //				Debug.Log($"control to {entity.view.Controller.Name}, guid {entity.id.Id}, control passed: {controlPassed}");
                if (controlPassed)
				{
					entity.isFinishedTurn = true;
					_context.isWaitingForInput = false;
                }
				else
				{
                    if (entity.isPlayerControlled)
					{
						_context.isWaitingForInput = true;
					}
				}
                entity.isEnergyReady = false;

            }
        }
	}
}
