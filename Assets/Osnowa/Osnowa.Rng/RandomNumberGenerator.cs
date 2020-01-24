namespace Osnowa.Osnowa.Rng
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using Zenject;

	public class RandomNumberGenerator : IRandomNumberGenerator
	{
		private Random _random;

		public RandomNumberGenerator()
		{
			_random = new Random();
		}

		[Inject]
		public RandomNumberGenerator(int seed)
		{
			_random = new Random(seed);
		}

		public int Next(int maxExclusive)
		{
			return _random.Next(maxExclusive);
		}

		public int Next(int minInclusive, int maxExclusive)
		{
			return _random.Next(minInclusive, maxExclusive);
		}

		public float NextFloat()
		{
			return (float)_random.NextDouble();
		}

		public TElement Choice<TElement>(IList<TElement> elementCollection)
		{
			int randomIndex = _random.Next(elementCollection.Count);
			if (elementCollection.Count == 0)
			{
				throw new ArgumentException("Collection for random element choice shouldn't be empty.");
			}
			TElement randomElement = elementCollection[randomIndex];
			return randomElement;
		}

		public IList<TElement> Shuffle<TElement>(IList<TElement> elements)
		{
			return elements.OrderBy(e => NextFloat()).ToList();
		}

		public bool Check(float chance)
		{
			float outcome = NextFloat();
			return outcome < chance;
		}

		public TimeSpan NextTimeSpan(TimeSpan maxSpanExclusive)
		{
			double maxSeconds = maxSpanExclusive.TotalSeconds;
			double randomSeconds = NextFloat() * maxSeconds;
			return TimeSpan.FromSeconds(randomSeconds);
		}

		public Position NextPosition(Bounds gridBounds)
		{
			int x = Next(gridBounds.Min.x, gridBounds.Max.x + 1);
			int y = Next(gridBounds.Min.y, gridBounds.Max.y + 1);
			return new Position(x, y);
		}

		public Position NextPosition(int maxXExclusive, int maxYExclusive)
		{
			return new Position(Next(maxXExclusive), Next(maxYExclusive));
		}

		public Position NextPosition(int minXInclusive, int minYInclusive, int maxXExclusive, int maxYExclusive)
		{
			return new Position(Next(minXInclusive, maxXExclusive), Next(minYInclusive, maxYExclusive));
		}

		public Position NextPosition(Position center, int radius)
		{
			bool inCircle = false;
			Position chosenPosition;
			do
			{
				chosenPosition = NextPosition(center.x - radius, center.y - radius, center.x + radius, center.y + radius);
				if (Position.Distance(chosenPosition, center) <= radius) inCircle = true;
			} while (!inCircle);
			return chosenPosition;
		}

		public Position BiasedPosition(Position centralPosition, int maxRange)
		{
			Bounds areaBounds = Bounds.CenteredOn(centralPosition, maxRange);
			Position candidatePosition;
			do
			{
				candidatePosition = NextPosition(areaBounds);
			} while (Position.Distance(centralPosition, candidatePosition) > maxRange);

			return candidatePosition;
		}

		public int Sign()
		{
			return _random.NextDouble() > 0.5f ? 1 : -1;
		}

		public void ResetWithSeed(int seed)
		{
			_random = new Random(seed);
		}
	}
}
