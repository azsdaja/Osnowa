namespace Osnowa
{
	using GameLogic;
	using GameLogic.Entities;
	using Osnowa.Context;
	using PCG.Recipees;
	using UI;

	public class DeathHandler : IDeathHandler
	{
		private readonly IUiFacade _uiFacade;
		private readonly IViewCreator _viewCreator;
		private readonly IGameConfig _gameConfig;
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IOsnowaContextManager _contextManager;


	    public DeathHandler(IUiFacade uiFacade, IViewCreator viewCreator, IGameConfig gameConfig, IPositionEffectPresenter positionEffectPresenter, IOsnowaContextManager contextManager)
		{
			_uiFacade = uiFacade;
			_viewCreator = viewCreator;
			_gameConfig = gameConfig;

		    _positionEffectPresenter = positionEffectPresenter;
		    _contextManager = contextManager;
		}
	
		public void HandleDeath(GameEntity entity)
		{
			entity.isMarkedForDestruction = true;

			if (_contextManager.Current.VisibleEntities.Contains(entity.view.Controller))
			{
				string name = entity.view.Controller.Name == "Player" ? "You" : entity.view.Controller.Name;
				string s = name == "You" ? "" : "s";
				_uiFacade.AddLogEntry(name + $" die{s}!");
			}

			EntityRecipee toSpawn = null;
			
			if (toSpawn != null)
			{
				_viewCreator.SpawnEntity(toSpawn, entity.position.Position);
			}

			if (entity.isPlayerControlled)
			{
				_uiFacade.HandlePlayerDeath("You die...");
			}
		}
	}
}