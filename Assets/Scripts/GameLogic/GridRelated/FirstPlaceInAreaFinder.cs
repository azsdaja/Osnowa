namespace GameLogic.GridRelated
{
	using System.Collections.Generic;
	using System.Linq;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Rng;
	using UnityUtilities;

	public class FirstPlaceInAreaFinder : IFirstPlaceInAreaFinder
	{
		private readonly IGrid _grid;
		private readonly IEntityDetector _entityDetector;
		private readonly IRandomNumberGenerator _rng;

		public FirstPlaceInAreaFinder(IGrid grid, IEntityDetector entityDetector, IRandomNumberGenerator rng)
		{
			_grid = grid;
			_entityDetector = entityDetector;
			_rng = rng;
		}

		public Position? FindForItem(Position source)
		{
			List<GameEntity> allAtSource = _entityDetector.DetectEntities(source).ToList();
			bool sourceIsEligible = _grid.IsWalkable(source) && allAtSource.All(e => e.hasHeld || e.isBlockingPosition);
			if (sourceIsEligible)
			{
				return source;	
			};
			IList<Position> neighboursShuffled = _rng.Shuffle(PositionUtilities.Neighbours8(source));
			foreach (Position neighbour in neighboursShuffled)
			{
				bool neighbourIsEligible = _grid.IsWalkable(neighbour) && !_entityDetector.DetectEntities(neighbour).Any();
				if (neighbourIsEligible)
				{
					return neighbour;
				}
			}

			return null;
		}

		public Position? FindForActor(Position source)
		{
			bool sourceIsEligible = _grid.IsWalkable(source) && !_entityDetector.DetectEntities(source).Any();
			if (sourceIsEligible)
			{
				return source;
			};
			IList<Position> neighboursShuffled = _rng.Shuffle(PositionUtilities.Neighbours8(source));
			foreach (Position neighbour in neighboursShuffled)
			{
				bool neighbourIsEligible = _grid.IsWalkable(neighbour) && !_entityDetector.DetectEntities(neighbour).Any();
				if (neighbourIsEligible)
				{
					return neighbour;
				}
			}

			return null;
		}
	}
}