namespace Osnowa.Osnowa.Core.ActionLoop
{
	/// <summary>
	/// Some executable logic related to showing effects of an action, such as moving actors, 
	/// playing animations, showing and hiding entities and so on. ActorData and other game data should not
	/// be affected by an action effect. Instead they should be updated in the execution of <see cref="IGameAction{SpecificActorData}"/>.
	/// </summary>
	public interface IActionEffect
	{
		/// <summary>
		/// Executes the given effect.
		/// </summary>
		void Process();
	}
}