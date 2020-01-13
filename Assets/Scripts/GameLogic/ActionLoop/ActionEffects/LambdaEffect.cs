namespace GameLogic.ActionLoop.ActionEffects
{
	using System;
	using Entities;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class LambdaEffect : IActionEffect
	{
		private readonly IViewController _viewController;
		public Action<IViewController> EffectAction { get; private set; }

		public LambdaEffect(Action<IViewController> effectAction, IViewController viewController)
		{
			_viewController = viewController;
			EffectAction = effectAction;
		}

		public virtual void Process()
		{
			EffectAction(_viewController);
		}
	}
}