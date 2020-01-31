namespace GameLogic.GameCore
{
	using System.Collections.Generic;
	using System.Linq;
	using Osnowa;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Tiles;
	using UI;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using UnityUtilities;

	/// <summary>
	/// Work in progress!
	/// </summary>
	public class EnvironmentPresenter : IEnvironmentPresenter
	{
		private readonly ISceneContext _sceneContext;
		
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IEntityDetector _entityDetector;
		private readonly ITilePresenter _tilePresenter;
		private readonly IGrid _grid;

		private readonly IList<PositionEffect> _positionEffectsShown;
		private readonly float _defaultUnseenMaskAlpha;
		private readonly IGameConfig _gameConfig;
		private readonly GameContext _context;
		private Tilemap _stealthTilemap;

		public EnvironmentPresenter(ISceneContext sceneContext, IPositionEffectPresenter positionEffectPresenter, 
			IEntityDetector entityDetector, ITilePresenter tilePresenter, IGameConfig gameConfig, 
			IGrid gipGrid, GameContext context)
		{
			_sceneContext = sceneContext;
			
			_positionEffectPresenter = positionEffectPresenter;
			_entityDetector = entityDetector;
			_tilePresenter = tilePresenter;
			_gameConfig = gameConfig;
			_grid = gipGrid;
			_context = context;

			_positionEffectsShown = new List<PositionEffect>();
			_defaultUnseenMaskAlpha = _sceneContext.UnseenMaskTilemap.color.a;
		}

		public Tilemap StealthTilemap => _stealthTilemap 
			? _stealthTilemap 
			: _stealthTilemap = PoolingManager.Fetch(PoolingManager.EffectTilemap, _sceneContext.TilemapDefiningOuterBounds.transform.position,
				Quaternion.identity, _sceneContext.EffectTilemapsParent).GetComponent<Tilemap>();

		public void ShowPlayerDetails()
		{
			GameEntity playerActor = _context.GetPlayerEntity();
			Position playerPosition = playerActor.position.Position;
			if (playerActor.isExecutePreTurn)
			{
				// this keeps starting and stopping while alt key is pressed!
				StopShowingCharacterDetails();
			}

			ShowActivitiesOfVisibleActors(playerPosition, playerActor);
			_tilePresenter.ShortenHighTiles(playerPosition, 30);
		}

		public void StopShowingCharacterDetails()
		{
			foreach (PositionEffect positionEffect in _positionEffectsShown)
			{
				PoolingManager.Free(PoolingManager.PositionEffect, positionEffect.gameObject);
			}
			_positionEffectsShown.Clear();

			GameEntity playerActor = _context.GetPlayerEntity();
			Position playerPosition = playerActor.position.Position;
			_tilePresenter.ResetToHighTiles();
			_tilePresenter.ShortenHighTiles(playerPosition, 5);
		}

		public void ShowStealthDetails()
		{
			GameEntity playerActor = _context.GetPlayerEntity();
			Position playerPosition = playerActor.position.Position;

			if (playerActor.isExecutePreTurn)
			{
				// this keeps starting and stopping while alt key is pressed!
				StopShowingStealthDetails();
			}

			_tilePresenter.ShortenHighTiles(playerPosition, 30);
			_sceneContext.UnseenMaskTilemap.color = ColorUtilities.WithAlpha(_sceneContext.UnseenMaskTilemap.color, 1f);
        }

		public void StopShowingStealthDetails()
		{
			GameEntity playerActor = _context.GetPlayerEntity();
			Position playerPosition = playerActor.position.Position;
			_tilePresenter.ResetToHighTiles();
			_tilePresenter.ShortenHighTiles(playerPosition, 5);

			StealthTilemap.ClearAllTiles();
			StealthTilemap.gameObject.SetActive(false);

			_sceneContext.UnseenMaskTilemap.color = ColorUtilities.WithAlpha(_sceneContext.UnseenMaskTilemap.color, _defaultUnseenMaskAlpha);
		}

		private void ShowActivitiesOfVisibleActors(Position playerPosition, GameEntity entity)
		{
			var visibleActors =
				_entityDetector.DetectEntities(playerPosition, 25).Where(a => a != entity /*&& a.Entity.IsVisible toecs*/).ToList();

			foreach (GameEntity visibleActor in visibleActors)
			{
				//toecs PositionEffect effect = _positionEffectPresenter.ShowStablePositionEffect(visibleActor.position.Position,
				//	visibleActor.Activity?.GetFullName() ?? "-", Color.yellow);
				//_positionEffectsShown.Add(effect);
			}
		}
    }
}