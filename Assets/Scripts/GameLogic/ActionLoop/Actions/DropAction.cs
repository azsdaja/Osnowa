namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using GridRelated;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;
	using UI;

	public class DropAction : GameAction
	{
		private readonly GameEntity _itemToDrop;
		private readonly IFirstPlaceInAreaFinder _firstPlaceInAreaFinder;
		private readonly IEntityDetector _entityDetector;
		private readonly IGameConfig _gameConfig;
		private readonly IUiFacade _uiFacade;
		private readonly IPositionEffectPresenter _positionEffectPresenter;

		public DropAction(GameEntity entity, float energyCost, GameEntity itemToDrop, IActionEffectFactory actionEffectFactory, IFirstPlaceInAreaFinder firstPlaceInAreaFinder, IEntityDetector entityDetector, IGameConfig gameConfig, IUiFacade uiFacade, IPositionEffectPresenter positionEffectPresenter) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_itemToDrop = itemToDrop;
			_firstPlaceInAreaFinder = firstPlaceInAreaFinder;
			_entityDetector = entityDetector;
			_gameConfig = gameConfig;
			_uiFacade = uiFacade;
			_positionEffectPresenter = positionEffectPresenter;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			if (Entity.entityHolder.EntityHeld == _itemToDrop.id.Id)
			{
				IActionEffect dropEffect = ActionEffectFactory
					.CreateLambdaEffect(actorBehaviour => actorBehaviour.DropHeldEntity(_itemToDrop), Entity.view.Controller);

				Entity.ReplaceEntityHolder(Guid.Empty);

				string itemName = _itemToDrop.view.Controller.Name;
				
				Position? newPosition = _firstPlaceInAreaFinder.FindForItem(Entity.position.Position);
				_itemToDrop.RemoveHeld();
				_itemToDrop.ReplacePosition(newPosition ?? Entity.position.Position);
				_itemToDrop.view.Controller.RefreshWorldPosition();
				_uiFacade.AddLogEntry($"<color=#aaa>You drop {itemName} on the ground.</color>");

				yield return dropEffect;
			}
		}
	}
}