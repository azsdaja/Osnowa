namespace Osnowa.Osnowa.RNG
{
	using System;
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// Produces random data matching given constraints.
	/// </summary>
	public interface IRandomNumberGenerator
	{
		/// <summary>
		/// Returns a random integer between zero (inclusive) and maxExclusive.
		/// </summary>
		int Next(int maxExclusive);

		/// <summary>
		/// Returns a random integer between minInclusive and maxExclusive.
		/// </summary>
		int Next(int minInclusive, int maxExclusive);

		/// <summary>
		/// Returns a random float between 0 (inclusive) and 1 (exclusive).
		/// </summary>
		float NextFloat();
	
		/// <summary>
		/// Returns a random element from given list.
		/// </summary>
		TElement Choice<TElement>(IList<TElement> elementCollection);

		/// <summary>
		/// Returns input list randomly reordered.
		/// </summary>
		IList<TElement> Shuffle<TElement>(IList<TElement> elements);

		/// <summary>
		/// Randomly returns true with chance matching the input.
		/// </summary>
		bool Check(float chance);

		/// <summary>
		/// Returns a random time span between 0 (inclusive) and maxSpanExclusive.
		/// </summary>
		TimeSpan NextTimeSpan(TimeSpan maxSpanExclusive);

		/// <summary>
		/// Returns a random position contained in given bounds. Warning: BoundsInt.max doesn't belong to the bounds!
		/// </summary>
		Position NextPosition(Bounds gridBounds);

		/// <summary>
		/// Returns a random position contained in given range.
		/// </summary>
		Position NextPosition(int maxXExclusive, int maxYExclusive);

		/// <summary>
		/// Returns a random position contained in given range.
		/// </summary>
		Position NextPosition(int minXInclusive, int minYInclusive, int maxXExclusive, int maxYExclusive);

		/// <summary>
		/// Returns a random position contained in given circle.
		/// </summary>
		Position NextPosition(Position center, int radius);

		/// <summary>
		/// Returns a random position which is no farther from given central position then by given maximum range.
		/// </summary>
		Position BiasedPosition(Position centralPosition, int maxRange);

		/// <summary>
		/// Returns -1 or 1 with equal chance.
		/// </summary>
		int Sign();

		/// <summary>
		/// Makes the RNG generate numbers according to given seed.
		/// </summary>
		void ResetWithSeed(int seed);
	}
}