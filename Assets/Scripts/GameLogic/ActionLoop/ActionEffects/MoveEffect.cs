namespace GameLogic.ActionLoop.ActionEffects
{
	using System;
	using System.Linq;
	using Animation;
	using Entities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;

	public class MoveEffect : IActionEffect
	{
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;
		private readonly IEntityDetector _entityDetector;
		public Position PreviousPosition { get; private set; }

		private GameEntity _entity;

		public MoveEffect(GameEntity entity, Position previousPosition, IUnityGridInfoProvider unityGridInfoProvider, 
			IEntityDetector entityDetector)
		{
			_entity = entity;
			PreviousPosition = previousPosition;
			_unityGridInfoProvider = unityGridInfoProvider;
			_entityDetector = entityDetector;
		}

		public virtual void Process()
		{
			Position position = _entity.position.Position;
			bool visibleActorsAreClose = _entity.isPlayerControlled &&
			                             _entityDetector.DetectEntities(position, 4)
				                             .Any(e => e != _entity && e.hasEnergy && e.hasView && e.view.Controller.IsVisible);
			if (visibleActorsAreClose)
			{
				DateTime potentialBlockedUntil = DateTime.UtcNow + TimeSpan.FromMilliseconds(150);
				if (_entity.hasBlockedUntil)
				{
					if (potentialBlockedUntil > _entity.blockedUntil.BlockedUntil)
						_entity.ReplaceBlockedUntil(potentialBlockedUntil);
				}
				else
					_entity.AddBlockedUntil(potentialBlockedUntil);
			}

			IViewController view = _entity.view.Controller;
			IEntityAnimator entityAnimator = view.Animator;

			if (view.IsVisible)
			{
				bool playerActorIsNear = _entityDetector.DetectEntities(position, 5).Any(a => a.isPlayerControlled);
				if (playerActorIsNear)
					entityAnimator.MoveTo(PreviousPosition, position);
			}
			Vector3 animationTargetPosition = _unityGridInfoProvider.GetCellCenterWorld(position);

			view.Transform.position = animationTargetPosition;
		}
	}
}