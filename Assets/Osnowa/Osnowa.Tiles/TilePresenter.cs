namespace Osnowa.Osnowa.Tiles
{
	using System.Collections.Generic;
	using Core;
	using GameLogic.GridRelated;
	using Context;
	using Unity.Tiles;
	using Unity.Tiles.Scripts;
	using UnityEngine;
	using UnityUtilities;

	public class TilePresenter : ITilePresenter
	{
		private readonly IGameConfig _gameConfig;
		private readonly ISceneContext _sceneContext;
		private readonly IOsnowaContextManager _contextManager;
		private readonly ITileByIdProvider _tileByIdProvider;
		private readonly ITileMatrixUpdater _tileMatrixUpdater;

		private readonly List<Position> _positionsToReset;

		public TilePresenter(ISceneContext sceneContext, IGameConfig gameConfig, IOsnowaContextManager contextManager, 
			ITileByIdProvider tileByIdProvider, ITileMatrixUpdater tileMatrixUpdater)
		{
			_gameConfig = gameConfig;
			_sceneContext = sceneContext;
			_contextManager = contextManager;
			_tileByIdProvider = tileByIdProvider;
			_tileMatrixUpdater = tileMatrixUpdater;

			_positionsToReset = new List<Position>(400);
		}

		internal HashSet<Position> LitPositionsSaved => _sceneContext.VisiblePositions;

		public void UpdateVisibility(HashSet<Position> visiblePositions)
		{
			foreach (Position oldPosition in LitPositionsSaved)
			{
				if (visiblePositions.Contains(oldPosition)) continue;
				SetUnseenMask(oldPosition);
			}
			foreach (Position currentPosition in visiblePositions)
			{
				if (LitPositionsSaved.Contains(currentPosition)) continue;
				RemoveUnseenMask(currentPosition);
			}

			_sceneContext.VisiblePositions = visiblePositions;
		}

		public void ShortenHighTiles(Position playerPosition, int range)
		{
			var bounds = new BoundsInt(playerPosition.x - range, playerPosition.y - range, 0, range*2+1, range*2+1, 1);
			MatrixByte standingTileMatrix = _contextManager.Current.TileMatricesByLayer[(int)TilemapLayer.Standing];
			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds();
			foreach (Vector3Int position3 in bounds.allPositionsWithin)
			{
				var position = position3.ToPosition();
                if (!standingTileMatrix.IsWithinBounds(position))
                    continue;
				byte standingTileAtPosition = standingTileMatrix.Get(position);
				if (standingTileAtPosition <= 0)
					continue;
				OsnowaBaseTile baseTileAtPosition = tilesByIds[standingTileAtPosition];
				if (baseTileAtPosition.ShorterVariant != null)
				{
					_positionsToReset.Add(position);
					_sceneContext.StandingTilemap.SetTile(position3, baseTileAtPosition.ShorterVariant);
				}
			}
		}

		public void ResetToHighTiles()
		{
			TilemapLayer standingLayer = TilemapLayer.Standing;

			foreach (Position position in _positionsToReset)
			{
				_tileMatrixUpdater.RefreshTile(position, (int) standingLayer);
			}
			_positionsToReset.Clear();
		}

		public virtual void SetUnseenMask(Position toSet)
		{
			_sceneContext.UnseenMaskTilemap.SetTile(toSet.ToVector3Int(), _gameConfig.Tileset.UnseenMask);

		}

		internal virtual void RemoveUnseenMask(Position toRemove)
		{
			_sceneContext.UnseenMaskTilemap.SetTile(toRemove.ToVector3Int(), null);
			_sceneContext.FogOfWarTilemap.SetTile(toRemove.ToVector3Int(), null);
		}
	}
}