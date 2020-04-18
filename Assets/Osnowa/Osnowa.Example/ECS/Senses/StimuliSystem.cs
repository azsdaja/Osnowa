namespace Osnowa.Osnowa.Example.ECS.Senses
{
    using System.Collections.Generic;
    using Entitas;
    using GameLogic.AI;
    using GameLogic.AI.Model;
    using GameLogic.Entities;

    public class StimuliSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _context;
        private readonly IStimulusHandler _stimulusHandler;
        private readonly IFriendshipResolver _friendshipResolver;

        public StimuliSystem(GameContext context, IStimulusHandler stimulusHandler, IFriendshipResolver friendshipResolver) : base(context)
        {
            _context = context;
            _stimulusHandler = stimulusHandler;
            _friendshipResolver = friendshipResolver;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Energy, GameMatcher.EnergyReady, GameMatcher.Stimuli));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasEnergy && entity.isEnergyReady && entity.hasStimuli;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                foreach (Stimulus stimulus in entity.stimuli.Stimuli)
                {
                    GameEntity emitterEntity = _context.GetEntityWithId(stimulus.ObjectEntityId);
                    if (stimulus.Type == StimulusType.INotice)
                    {
                        bool areEnemies = !_friendshipResolver.AreFriends(entity, emitterEntity);
                        _stimulusHandler.Notice(entity, stimulus, areEnemies);  
                    }
                }
            }
        }
    }
}