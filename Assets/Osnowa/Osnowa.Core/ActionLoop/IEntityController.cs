namespace Osnowa.Osnowa.Core.ActionLoop
{
	public interface IEntityController
	{
		/// <summary>Updates an actor's control data, resolves his action and executes the action and its effects. </summary>
		/// <returns>True when control should be switched to next actor (when current actor is regaining energy or is performing an action).
		/// False when control should remain at current actor.</returns>
		bool GiveControl(GameEntity entity);
	}
}