namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using GridRelated;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class ReleaseAction : GameAction
	{
		private readonly IFirstPlaceInAreaFinder _firstPlaceInAreaFinder;

		public ReleaseAction(GameEntity entity, float energyCost, IFirstPlaceInAreaFinder firstPlaceInAreaFinder, 
			IActionEffectFactory actionEffectFactory) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_firstPlaceInAreaFinder = firstPlaceInAreaFinder;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			GameEntity entityToRelease = Contexts.sharedInstance.game.GetEntityWithId(Entity.entityHolder.EntityHeld);

			Position? newPosition = _firstPlaceInAreaFinder.FindForItem(Entity.position.Position);
			entityToRelease.ReplacePosition(newPosition.Value);
			entityToRelease.AddEnergy(1f, 0f);
			entityToRelease.isCarryable = true;

			Entity.ReplaceEntityHolder(Guid.Empty);
			IActionEffect effect = ActionEffectFactory
					.CreateLambdaEffect(viewController => viewController.DropHeldEntity(entityToRelease), Entity.view.Controller);

			yield return effect;
		}
	}
}