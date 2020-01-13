namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class LambdaAction : GameAction
	{
		private readonly Func<GameEntity, IEnumerable<IActionEffect>> _inlineAction;

		public LambdaAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory, 
			Func<GameEntity, IEnumerable<IActionEffect>> inlineAction) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_inlineAction = inlineAction;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			return _inlineAction(Entity);
		}
	}
}