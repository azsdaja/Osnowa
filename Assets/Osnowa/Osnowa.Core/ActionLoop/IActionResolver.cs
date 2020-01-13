namespace Osnowa.Osnowa.Core.ActionLoop
{
	public interface IActionResolver
	{
		/// <summary>
		/// Decides what action the actor is going to perform next.
		/// </summary>
		IGameAction GetAction(GameEntity entity);

		IGameAction CreateTakeFromInventoryActionIfPossible(GameEntity entity, int index);
		IGameAction CreateTakeToInventoryActionIfPossible(GameEntity entity);
		IGameAction CreateDropActionIfPossible(GameEntity playerEntity);
	}
}