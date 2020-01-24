namespace GameLogic.ActionLoop.Actions
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Example.ECS;
	using Osnowa.Osnowa.Rng;
	using UI;

	public class AttackAction : GameAction
	{
		private readonly IRandomNumberGenerator _rng;
		private readonly IActionEffectFactory _actionEffectFactory;
		private readonly IGameConfig _gameConfig;
		private readonly IOsnowaContextManager _contextManager;
        private readonly ReactiveFeature _reactiveFeature;

        public AttackAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory, IRandomNumberGenerator rng, GameEntity attackedEntity, IGameConfig gameConfig, IAggressionTriggerer aggressionTriggerer, IPositionEffectPresenter positionEffectPresenter, IOsnowaContextManager contextManager, ReactiveFeature reactiveFeature)
			: base(entity, energyCost, actionEffectFactory)
		{
			AttackedEntity = attackedEntity;
			_rng = rng;
			_actionEffectFactory = actionEffectFactory;
			_gameConfig = gameConfig;
			_contextManager = contextManager;
            _reactiveFeature = reactiveFeature;
        }

		internal GameEntity AttackedEntity { get; }

		public override IEnumerable<IActionEffect> Execute()
		{
			int maxDamage = 10;
			if (Entity.isPlayerControlled)
			{
				maxDamage = (int) (maxDamage);
			}
			int damage = _rng.Next(1, maxDamage + 1);
            AttackedEntity.ReplaceReceiveDamage(damage, Entity.id.Id);
            _reactiveFeature.Execute();

            IActionEffect bumpEffect = _actionEffectFactory.CreateBumpEffect(Entity, AttackedEntity.position.Position);
			yield return bumpEffect;
		}
	}
} 