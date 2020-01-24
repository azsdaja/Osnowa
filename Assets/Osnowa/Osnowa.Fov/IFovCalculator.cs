namespace Osnowa.Osnowa.Fov
{
	using System;
	using System.Collections.Generic;
	using Core;

	public interface IFovCalculator
	{
		/// <summary>
		/// Calculates the set of visible tiles basing on observer position, his range of sight and information about blocking tiles.
		/// </summary>
		HashSet<Position> CalculateFov(Position observerPosition, int sightRange, Func<Position, bool> isPassingLight);
	}
}