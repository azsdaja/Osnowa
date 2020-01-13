namespace Osnowa.Osnowa.Core.ActionLoop
{
	using System;

	/// <summary>
	/// Contains all the necessary information needed for deciding when control (a turn) should be given to an actor.
	/// </summary>
	[Serializable]
	public class ActorControlData
	{
		/// <summary>
		/// A ready-to-execute action which the actor is going to execute when he becomes unblocked (e.g. after playing an animation).
		/// </summary>
		public IGameAction StoredAction { get; set; }
		
		/// <summary>
		/// Indicates until when the actor should abstain from performing actions.
		/// </summary>
		public DateTime BlockedUntil { get; set; }
	}
}