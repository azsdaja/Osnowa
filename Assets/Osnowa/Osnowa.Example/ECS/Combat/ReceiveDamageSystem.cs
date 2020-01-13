using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Osnowa.Osnowa.Example.ECS.Combat
{
	using GameLogic;
	using GameLogic.ActionLoop.Actions;
	using UI;

	public class ReceiveDamageSystem : ReactiveSystem<GameEntity>
	{
		private readonly IDeathHandler _deathHandler;
		private readonly IUiFacade _uiFacade;
        private readonly IAggressionTriggerer _aggressionTriggerer;
        private readonly IPositionEffectPresenter _positionEffectPresenter;

        public ReceiveDamageSystem(IContext<GameEntity> context, IDeathHandler deathHandler, IUiFacade uiFacade, 
            IAggressionTriggerer aggressionTriggerer, IPositionEffectPresenter positionEffectPresenter) : base(context)
		{
			_deathHandler = deathHandler;
			_uiFacade = uiFacade;
            _aggressionTriggerer = aggressionTriggerer;
            _positionEffectPresenter = positionEffectPresenter;
        }

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.ReceiveDamage);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasReceiveDamage;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
            {
                int damage = entity.receiveDamage.DamageReceived;

                if (entity.hasIntegrity)
                {
                    entity.ReplaceIntegrity(entity.integrity.Integrity - damage, entity.integrity.MaxIntegrity);
                }

                _positionEffectPresenter.ShowPositionEffect(entity.position.Position, "-" + damage, Color.red, false, 1f);

                _aggressionTriggerer.TriggerAggressionIfEligible(entity);

                entity.RemoveReceiveDamage();
            }
		}
	}
}