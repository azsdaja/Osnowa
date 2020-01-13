namespace GameLogic.ActionLoop.Actions
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	public abstract class DirectedAction : GameAction
	{
		public Position Direction { get; protected set; }

		protected DirectedAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory, Position direction) 
			: base(entity, energyCost, actionEffectFactory)
		{
			Direction = direction;
		}
	}
}
