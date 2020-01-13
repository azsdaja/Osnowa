using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Lifetime
{
    public class DeathClockSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;

        public DeathClockSystem(GameContext context) : base(context)
        {
            _context = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.DeathClock);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasDeathClock;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                entity.ReplaceDeathClock(entity.deathClock.TurnsLeft - 1);
                if (entity.deathClock.TurnsLeft <= 0)
                {
                    entity.isMarkedForDestruction = true;
                }
            }
        }
    }
}