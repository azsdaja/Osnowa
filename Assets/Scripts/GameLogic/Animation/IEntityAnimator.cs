namespace GameLogic.Animation
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public interface IEntityAnimator
	{
		Animator BodyAnimator { get; }

		/// <summary>
		/// Indicates if an animation is being played on an entity.
		/// </summary>
		bool IsAnimating { get; }

		/// <summary>
		/// Transform component of game object containing all visible objects that make up the actor.
		/// </summary>
		Transform MyVisuals { get; }

		void MoveTo(Position sourceLogicalPosition, Position targetLogicalPosition);
		void Bump(Position sourceLogicalPosition, Position affectedPosition);
		void KnockOut();
		void StandUp();

		/// <summary>
		/// Finishes the currently played animation.
		/// </summary>
		void FinishedAnimating();
	}
}