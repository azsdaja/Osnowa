namespace GameLogic.ActionLoop.ActionEffects
{
	using Animation;
	using Entities;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class KnockOutEffect : IActionEffect
	{
		private readonly GameEntity _entity;

		public KnockOutEffect(GameEntity entity)
		{
			_entity = entity;
		}

		public virtual void Process()
		{
			IViewController view = _entity.view.Controller;
			IEntityAnimator entityAnimator = view.Animator;
			bool actorIsVisible = view.IsVisible;
			if (actorIsVisible)
				entityAnimator.KnockOut();
		}
	}
}
