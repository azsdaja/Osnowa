using System.Collections.Generic;
using Entitas;
using Osnowa.Osnowa.Grid;

namespace Osnowa.Osnowa.Example.ECS.Heartbeat
{
	using Tiles;

	public class PresentSurroundingsSystem : ReactiveSystem<GameEntity>
	{
		private readonly IGameConfig _gameConfig;
		
		private readonly ITilePresenter _tilePresenter;
		private readonly IVisibilityUpdater _visibilityUpdater;

		public PresentSurroundingsSystem(IGameConfig gameConfig, IContext<GameEntity> context, ITilePresenter tilePresenter, IVisibilityUpdater visibilityUpdater) 
			: base(context)
		{
			_gameConfig = gameConfig;
			
			_tilePresenter = tilePresenter;
			_visibilityUpdater = visibilityUpdater;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.PlayerControlled, GameMatcher.Position, GameMatcher.ExecutePreTurn,
				GameMatcher.Vision));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isPlayerControlled && entity.hasPosition && entity.isExecutePreTurn && entity.hasVision;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				_tilePresenter.ResetToHighTiles();
				_tilePresenter.ShortenHighTiles(entity.position.Position, 5);
				if (_gameConfig.ModeConfig.Vision != Vision.Permanent)
				{
					_visibilityUpdater.UpdateVisibility(entity.position.Position, entity.vision.VisionRange);
				}
			}
		}
	}
}