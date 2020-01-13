using System.Collections.Generic;

namespace Osnowa.Osnowa.Grid
{
	using Core;
	using FOV;
	using Tiles;

	public class VisibilityUpdater : IVisibilityUpdater
	{
	    private readonly IGrid _grid;
	    private readonly IFovCalculator _fovCalculator;
	    private readonly ITilePresenter _tilePresenter;
	    private readonly IEntityPresenter _entityPresenter;

	    public VisibilityUpdater(ITilePresenter tilePresenter, IEntityPresenter entityPresenter, IFovCalculator fovCalculator, IGrid grid)
	    {
	        _grid = grid;
            _fovCalculator = fovCalculator;
            _tilePresenter = tilePresenter;
			_entityPresenter = entityPresenter;
		}

		public void UpdateVisibility(Position observerPosition, int sightRange)
		{
            HashSet<Position> fieldOfView = _fovCalculator.CalculateFov(observerPosition, sightRange,
                    position => _grid.IsPassingLight(position) || position == observerPosition);

			_tilePresenter.UpdateVisibility(fieldOfView);
			_entityPresenter.UpdateVisibility(fieldOfView, observerPosition, sightRange);
		}
	}
}