namespace Osnowa.Osnowa.Core.ActionLoop
{
	using System;
	using GameLogic.Entities;

	/// <summary>
	/// A factory for producing action effects and passing them all the necessary dependencies.
	/// </summary>
	public interface IActionEffectFactory
	{
		IActionEffect CreateMoveEffect(GameEntity entity, Position activeActorPositionBefore);
		IActionEffect CreateLambdaEffect(Action<IViewController> effectAction, IViewController actorBehaviour);
		IActionEffect CreateBumpEffect(GameEntity entity, Position newPosition);
		IActionEffect CreateKnockoutEffect(GameEntity entity);
	}
}