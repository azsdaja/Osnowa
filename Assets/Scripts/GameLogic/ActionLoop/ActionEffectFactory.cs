namespace GameLogic.ActionLoop
{
	using System;
	using ActionEffects;
	using Entities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Unity;

	class ActionEffectFactory : IActionEffectFactory
	{
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;
		private readonly IEntityDetector _entityDetector;

		public ActionEffectFactory(IUnityGridInfoProvider unityGridInfoProvider, IEntityDetector entityDetector)
		{
			_unityGridInfoProvider = unityGridInfoProvider;
			_entityDetector = entityDetector;
		}

		public IActionEffect CreateMoveEffect(GameEntity entity, Position activeActorPositionBefore)
		{
			return new MoveEffect(entity, activeActorPositionBefore, _unityGridInfoProvider, _entityDetector);
		}

		public IActionEffect CreateLambdaEffect(Action<IViewController> effectAction, IViewController actorBehaviour)
		{
			return new LambdaEffect(effectAction, actorBehaviour);
		}

		public IActionEffect CreateBumpEffect(GameEntity entity, Position newPosition)
		{
			return new BumpEffect(entity, newPosition);
		}

		public IActionEffect CreateKnockoutEffect(GameEntity entity)
		{
			return new KnockOutEffect(entity);
		}
	}
}