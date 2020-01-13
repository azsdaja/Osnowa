namespace GameLogic.ActionLoop.Actions
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class DisplaceAction : GameAction
	{
		public GameEntity DisplacedEntity { get; }

		public DisplaceAction(GameEntity entity, GameEntity displacedEntity, float energyCost, 
			IActionEffectFactory actionEffectFactory) 
			: base(entity, energyCost, actionEffectFactory)
		{
			DisplacedEntity = displacedEntity;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			Position activeActorPositionBefore = Entity.position.Position;
			Position displacedActorPositionBefore = DisplacedEntity.position.Position;

			Entity.ReplacePosition(displacedActorPositionBefore);
			DisplacedEntity.ReplacePosition(activeActorPositionBefore);

			yield return ActionEffectFactory.CreateMoveEffect(Entity, activeActorPositionBefore);
			yield return ActionEffectFactory.CreateMoveEffect(Entity, displacedActorPositionBefore);
		}
	}
}
