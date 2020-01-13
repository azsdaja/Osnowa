namespace GameLogic.ActionLoop.ActionEffects
{
	using Animation;
	using Entities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class BumpEffect : IActionEffect
	{
		private readonly GameEntity _entity;

		public BumpEffect(GameEntity entity, Position bumpedPosition)
		{
			_entity = entity;
			BumpedPosition = bumpedPosition;
		}

		public Position BumpedPosition { get; private set; }

		public virtual void Process()
		{
			IViewController view = _entity.view.Controller;
			IEntityAnimator entityAnimator = view.Animator;
			bool actorIsVisible = view.IsVisible;
			if (actorIsVisible)
				entityAnimator.Bump(_entity.position.Position, BumpedPosition);
		}
	}
}