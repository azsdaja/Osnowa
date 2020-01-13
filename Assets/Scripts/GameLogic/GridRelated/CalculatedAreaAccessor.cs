namespace GameLogic.GridRelated
{
	using System.Collections.Generic;
	using FloodSpill;
	using FloodSpill.NeighbourProcessors;
	using Osnowa;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.FOV;
	using Osnowa.Osnowa.Grid;
	using Position = Osnowa.Osnowa.Core.Position;

	public class CalculatedAreaAccessor : ICalculatedAreaAccessor
	{
		private readonly IGrid _grid;
		private readonly IFovCalculator _fovCalculator;
		private readonly FloodSpiller _floodSpiller;

		private FloodArea _cachedFloodArea;
		private FovArea _cachedFovArea;

		public CalculatedAreaAccessor(IGrid grid, IFovCalculator fovCalculator, FloodSpiller floodSpiller)
		{
            _grid = grid;
			_fovCalculator = fovCalculator;
			_floodSpiller = floodSpiller;
		}

		public IFloodArea FetchWalkableFlood(Position center, int floodRange)
		{
			// performance: maybe we should keep a few last calculated results for center-floodRange pairs

			if (_cachedFloodArea == null)
			{
				_cachedFloodArea = new FloodArea(center, floodRange);
			}

			int expectedMatrixSize = floodRange * 2 + 1;
			if (_cachedFloodArea.ArraySize < expectedMatrixSize)
			{
				_cachedFloodArea.IncreaseMatrix(expectedMatrixSize);
			}

			if (_cachedFloodArea.Center != center)
			{
				_cachedFloodArea.Center = center;
				int boundsSize = floodRange * 2;
				_cachedFloodArea.Bounds = new Bounds(center.x - floodRange, center.y - floodRange, boundsSize, boundsSize);
			}

			var findHighestMark = new FindHighestMarkNeighbourProcessor();
			var parameters = new FloodParameters(center.x, center.y)
			{
				Qualifier = (x, y) => _grid.IsWalkable(new Position(x, y)),
				BoundsRestriction = BoundsUtilities.ToFloodBounds(_cachedFloodArea.Bounds),
				NeighbourProcessor = findHighestMark.Process
			};
			Position furthestPosition = center;
			_floodSpiller.SpillFlood(parameters, _cachedFloodArea.ValueMatrix);
			_cachedFloodArea.FurthestPosition = furthestPosition;

			return _cachedFloodArea;
		}

		public FovArea FetchVisibilityFov(Position center, int sightRange)
		{
			if (_cachedFovArea == null || _cachedFovArea.Center != center || _cachedFovArea.SightRange != sightRange)
            {
                HashSet<Position> fovPositions =  _fovCalculator.CalculateFov(center, sightRange, 
					position => _grid.IsPassingLight(position) || position == center);
                _cachedFovArea = new FovArea {Center = center, SightRange = sightRange, Positions = fovPositions};
            }

			return _cachedFovArea;
		}
	}
}