namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UI;

	public class EatItemAction : GameAction
	{
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly GameEntity _foodItemToEat;

		public EatItemAction(GameEntity entity, float energyCost, GameEntity foodItemToEat, 
			IActionEffectFactory actionEffectFactory, IPositionEffectPresenter positionEffectPresenter)
			: base(entity, energyCost, actionEffectFactory)
		{
			_positionEffectPresenter = positionEffectPresenter;
			_foodItemToEat = foodItemToEat;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			int bite = 15;
			int newSatiation = Math.Min(Entity.stomach.MaxSatiation, Entity.stomach.Satiation + bite);
			Entity.ReplaceStomach(newSatiation, Entity.stomach.MaxSatiation);
			int newSatiety = _foodItemToEat.edible.Satiety - bite;
			if (newSatiety > 0)
			{
				_foodItemToEat.ReplaceEdible(newSatiety);
			}
			else
			{
				// todo move this rather to a system
				
				_foodItemToEat.isMarkedForDestruction = true;
			}

			return new[]{new LambdaEffect(actorBehaviour =>
			{
				_positionEffectPresenter.ShowPositionEffect(Entity.position.Position, "Munch!");
			}, Entity.view.Controller)};
		}
	}
}