namespace GameLogic.ActionLoop
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;

	/// <summary>
	/// A factory for producing actions and passing them all the necessary dependencies.
	/// </summary>
	public interface IActionFactory
	{
		IGameAction CreateDisplaceAction(GameEntity entity, GameEntity actorAtTargetPosition);
		IGameAction CreateAttackAction(GameEntity entity, GameEntity attackedEntity);
		IGameAction CreateJustMoveAction(Position actionVector, GameEntity entity);
	    IGameAction CreateDropAction(GameEntity itemToDrop, GameEntity entity);
		IGameAction CreatePickUpAction(GameEntity itemToPickUp, GameEntity entity);
		IGameAction CreateTakeToInventoryAction(GameEntity entity);
		IGameAction CreatePassAction(GameEntity entity, float cost = 1f);
		IGameAction CreateMoveAction(Position direction, GameEntity entity);
	    IGameAction CreateEatAction(GameEntity consumedEntity, GameEntity entity);
	    IGameAction CreateLambdaAction(Func<GameEntity, IEnumerable<IActionEffect>> action, GameEntity entity, float cost = 1f);
	    IGameAction CreateKindapAction(GameEntity kidnappedEntity, GameEntity entity);
		IGameAction CreateReleaseAction(GameEntity entity);
		IGameAction CreateEatItemAction(GameEntity itemToEat, GameEntity entity);
		IGameAction CreateShoutWarningAction(GameEntity entity);
		IGameAction CreateTakeFromInventoryAction(GameEntity entity, int indexInInventory);
		IGameAction CreateSwapHandWithInventoryAction(GameEntity entity, int indexInInventory);
	}
}