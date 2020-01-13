namespace GameLogic.ActionLoop.Actions
{
	using System.Collections.Generic;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UI;

	public class EatAction : GameAction
	{
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly GameEntity _consumedIngredient;

		public EatAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory, IPositionEffectPresenter positionEffectPresenter, GameEntity consumedIngredient)
			: base(entity, energyCost, actionEffectFactory)
		{
			_positionEffectPresenter = positionEffectPresenter;
			_consumedIngredient = consumedIngredient;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			if(Entity.hasStomach) Entity.ReplaceStomach(Entity.stomach.Satiation + 50, Entity.stomach.MaxSatiation);

			_consumedIngredient.isMarkedForDestruction = true;

			return new[]{new LambdaEffect(actorBehaviour =>
			{
				_positionEffectPresenter.ShowPositionEffect(Entity.position.Position, "Gulp! Munch!");
			}, Entity.view.Controller)};
		}
	}
}
