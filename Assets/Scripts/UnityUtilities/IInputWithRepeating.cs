namespace UnityUtilities
{
	using UnityEngine;

	/// <summary>
	/// Detects input events with taking repeated commands (when a button is held) in consideration.
	/// </summary>
	public interface IInputWithRepeating
	{
		/// <summary>
		/// Returns true if the button just got pressed or if it's held and current time is matching the interval.
		/// </summary>
		bool KeyDownOrRepeating(KeyCode keyCode);

		/// <summary>
		/// If the button is pressed — returns true and updates the time of holding the button.
		/// </summary>
		bool GetAndUpdateKey(KeyCode keyCode);

		/// <summary>
		/// Resets the time counter of holding a button.
		/// </summary>
		void ResetTime();
	}
}