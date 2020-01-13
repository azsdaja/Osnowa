namespace Osnowa.Osnowa.Core.ActionLoop
{
	using System.Collections.Generic;

	public abstract class GameAction : IGameAction
	{
		protected readonly IActionEffectFactory ActionEffectFactory;

		public GameEntity Entity { get; }
		public float EnergyCost { get; }

		protected GameAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory)
		{
			Entity = entity;
			EnergyCost = energyCost;
			ActionEffectFactory = actionEffectFactory;
		}

		public abstract IEnumerable<IActionEffect> Execute();
	}
}