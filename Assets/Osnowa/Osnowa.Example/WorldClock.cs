namespace Osnowa.Osnowa.Example
{
	using System;
	using Context;
	using GameLogic;
	using GameLogic.Entities;
	using GameLogic.GridRelated;
	using global::Osnowa.Osnowa.Grid;
	using Rng;

	public class WorldClock : IWorldClock
	{
		private readonly ISceneContext _sceneContext;
		
		private readonly IGameConfig _gameConfig;
		private readonly IUiFacade _uiFacade;
		private readonly double _minutesInTurn;
		private readonly IEntityDetector _entityDetector;
		private readonly ITileMatrixUpdater _tileMatrixUpdater;
		private IRandomNumberGenerator _rng;
		private readonly IViewCreator _viewCreator;
		private GameContext _gameContext;
		private readonly IOsnowaContextManager _contextManager;

		public WorldClock(ISceneContext sceneContext, IGameConfig gameConfig,
			IUiFacade uiFacadeUiFacade, IEntityDetector entityDetector, ITileMatrixUpdater tileMatrixUpdater,
			IRandomNumberGenerator rng, IViewCreator viewCreator,
			IOsnowaContextManager contextManager)
		{
			_sceneContext = sceneContext;
			
			_gameConfig = gameConfig;
			_uiFacade = uiFacadeUiFacade;
			_entityDetector = entityDetector;
			_tileMatrixUpdater = tileMatrixUpdater;
			_rng = rng;
			_viewCreator = viewCreator;
			_contextManager = contextManager;
			_gameContext = Contexts.sharedInstance.game;

			double minutesInDay = new TimeSpan(1, 0, 0, 0).TotalMinutes;
			_minutesInTurn = minutesInDay/_gameConfig.TurnsInDay;
		}

		public void HandleSegment()
		{
			HandleTurn();
		}

		private void HandleTurn()
        {
            _contextManager.Current.TurnsPassed += 1;
            int turnsPassed = _contextManager.Current.TurnsPassed;

            if (_gameConfig.TurnsInDay <= 0)
            {
	            return;
            }
            float timeOfDayPercentage = (turnsPassed % _gameConfig.TurnsInDay)/(float) _gameConfig.TurnsInDay;
			HandleWorldColor(timeOfDayPercentage);
		}

	    private void HandleWorldColor(float timeOfDayPercentage)
		{
			var skyColor = _gameConfig.TimeOfDayToColor.Evaluate(timeOfDayPercentage);
            // todo doesn't work at the moment. Use 2D lighting package with Sprite-lit material for the tilemap or upgrade Unity and use
            // 2D lights.
			_sceneContext.SkyLight.color = skyColor;
		}
	}
}