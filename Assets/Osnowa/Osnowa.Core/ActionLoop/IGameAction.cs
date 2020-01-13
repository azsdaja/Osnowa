namespace Osnowa.Osnowa.Core.ActionLoop
{
	using System.Collections.Generic;

	/// <summary>
	/// Some executable logic related to updating the game state according to the performed action,
	/// for example changing actor's logical position, updating his stats and so on.
	/// Things like moving actor's GameObject in Unity or playing animations should not be 
	/// be executed here. Instead they should be done in <see cref="IActionEffect"/>s produced by a game action.
	/// </summary>
	public interface IGameAction
	{
		/// <summary>
		/// Data of the actor performing the action.
		/// </summary>
		GameEntity Entity { get; }

		/// <summary>
		/// Indicates how much energy an actor is deducted for performing given action.
		/// </summary>
		float EnergyCost { get; }

		/// <summary>
		/// Executes the action and produces its effects.
		/// </summary>
		IEnumerable<IActionEffect> Execute();
	}
}