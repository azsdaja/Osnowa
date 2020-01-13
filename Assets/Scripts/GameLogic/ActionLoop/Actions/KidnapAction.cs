namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class KidnapAction : GameAction
	{
		public GameEntity EntityToKidnap { get; }

		public KidnapAction(GameEntity entity, float energyCost, GameEntity entityToKidnap, 
			IActionEffectFactory actionEffectFactory) 
			: base(entity, energyCost, actionEffectFactory)
		{
			EntityToKidnap = entityToKidnap;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			Entity.ReplaceEntityHolder(EntityToKidnap.id.Id);
			EntityToKidnap.AddHeld(Entity.id.Id);
			EntityToKidnap.RemoveEnergy();
			EntityToKidnap.isEnergyReady = false;
			EntityToKidnap.isCarryable = false;

			if (!Entity.hasEntityHolder || Entity.entityHolder.EntityHeld == Guid.Empty)
			{
				IActionEffect effect = ActionEffectFactory
					.CreateLambdaEffect(viewController => viewController.HoldOnBack(EntityToKidnap), Entity.view.Controller);
				yield return effect;
			}
			else throw new InvalidOperationException("Kidnap action shouldn't be executed if the active actor is holding something.");
		}
	}
}