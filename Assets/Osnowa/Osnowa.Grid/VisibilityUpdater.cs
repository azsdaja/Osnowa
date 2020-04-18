using System.Collections.Generic;

namespace Osnowa.Osnowa.Grid
{
	using System.Linq;
	using Context;
	using Core;
	using Entities;
	using Fov;
	using Tiles;

	public class VisibilityUpdater : IVisibilityUpdater
	{
	    private readonly IGrid _grid;
	    private readonly IFovCalculator _fovCalculator;
	    private readonly ITilePresenter _tilePresenter;
	    private readonly IEntityPresenter _entityPresenter;
	    private readonly IOsnowaContextManager _contextManager;

	    public VisibilityUpdater(ITilePresenter tilePresenter, IEntityPresenter entityPresenter, IFovCalculator fovCalculator, IGrid grid, 
		    IOsnowaContextManager contextManager)
	    {
	        _grid = grid;
            _fovCalculator = fovCalculator;
            _tilePresenter = tilePresenter;
			_entityPresenter = entityPresenter;
			_contextManager = contextManager;
	    }

		public void UpdateVisibility(Position observerPosition, int sightRange, GameEntity entity)
		{
            HashSet<Position> fieldOfView = _fovCalculator.CalculateFov(observerPosition, sightRange,
                    position => _grid.IsPassingLight(position) || position == observerPosition);

			_tilePresenter.UpdateVisibility(fieldOfView);
			_entityPresenter.UpdateVisibility(fieldOfView, observerPosition, sightRange);
		}
	}
}