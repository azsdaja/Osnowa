namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;

	public class PickUpAction : GameAction
	{
		private readonly IEntityDetector _entityDetector;
		private readonly IUiFacade _uiFacade;
		public GameEntity ItemToPickUp { get; }

		public PickUpAction(GameEntity entity, float energyCost, GameEntity itemToPickUp, IActionEffectFactory actionEffectFactory, IEntityDetector entityDetector, IUiFacade uiFacade) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_entityDetector = entityDetector;
			ItemToPickUp = itemToPickUp;
			_uiFacade = uiFacade;
		}

		public override IEnumerable<IActionEffect> Execute()
	    {
	        if (Entity.entityHolder.EntityHeld != Guid.Empty || ItemToPickUp.hasHeld)
	        {
	            throw new InvalidOperationException(
	                "Actor should not try to pick up if he's holding an entity or someone else is holding an entity");
	        }

	        Entity.entityHolder.EntityHeld = ItemToPickUp.id.Id;
	        ItemToPickUp.ReplaceHeld(Entity.id.Id);

	        _uiFacade.AddLogEntry($"<color=#aaa>You pick up {ItemToPickUp.view.Controller.Name}.</color>");

	        IActionEffect effect = ActionEffectFactory
	            .CreateLambdaEffect(viewController => viewController.HoldOnFront(ItemToPickUp), Entity.view.Controller);
	        yield return effect;
	    }
	}
}
