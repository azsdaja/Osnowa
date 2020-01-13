using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Body
{
    public class HungerSystem : ReactiveSystem<GameEntity>
    {
        public HungerSystem(IContext<GameEntity> context) 
            : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Stomach, GameMatcher.ExecutePreTurn));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasStomach && entity.isExecutePreTurn;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                entity.ReplaceStomach(entity.stomach.Satiation - 1, entity.stomach.MaxSatiation);
            }
        }
    }
}