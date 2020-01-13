namespace GameLogic.ActionLoop.ActionEffects
{
	using Animation;
	using Osnowa.Osnowa.Core.ActionLoop;

	public class StandUpEffect : IActionEffect
	{
		private readonly GameEntity _entity;

		public StandUpEffect(GameEntity entity)
		{
			_entity = entity;
		}

		public void Process()
		{
			IEntityAnimator entityAnimator = _entity.view.Controller.Animator;
			bool actorIsVisible = _entity.view.Controller.IsVisible;
			if (actorIsVisible)
				entityAnimator.StandUp();
		}
	}
}