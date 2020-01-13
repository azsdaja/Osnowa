namespace UI
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;
	using UnityUtilities;

	/// <summary>
	/// Shows text effects. Dedicated mostly for debug purposes.
	/// </summary>
	public interface IPositionEffectPresenter
	{
		PositionEffect ShowPositionEffect(Position position, string text, Color? color = null, bool markPosition = false, float duration = 2.5f, float delay = 0.0f);
		PositionEffect ShowStablePositionEffect(Position position, string text, Color? color = null, bool markPosition = false);
	}
}