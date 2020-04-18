namespace GameLogic.GridRelated
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Unity.Tiles;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using UnityUtilities;

	public class TileMatrixUpdater : ITileMatrixUpdater
	{
		private readonly IPositionFlagsResolver _positionFlagsResolver;
		private readonly IGameConfig _gameConfig;
		private readonly ITileByIdProvider _tileByIdProvider;
		private readonly ISceneContext _sceneContext;
		private MatrixByte[] _tileMatricesByteByLayer;

		public TileMatrixUpdater(IPositionFlagsResolver positionFlagsResolver, IGameConfig gameConfig, 
			ITileByIdProvider tileByIdProvider, ISceneContext sceneContext, IOsnowaContextManager contextManager)
		{
			_positionFlagsResolver = positionFlagsResolver;
			_gameConfig = gameConfig;
			_tileByIdProvider = tileByIdProvider;
			_sceneContext = sceneContext;

			contextManager.ContextReplaced += newContext => _tileMatricesByteByLayer = newContext.TileMatricesByLayer;
		}

		public void Set(Position position, OsnowaBaseTile baseTileToSet)
		{
			int layerId = (int) baseTileToSet.Layer;
			_tileMatricesByteByLayer[layerId].Set(position, baseTileToSet.Id);

			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds();
				
			int tilesByIdsCount = tilesByIds.Length;

			_sceneContext.AllTilemapsByLayers[layerId].SetTile(position.ToVector3Int(), baseTileToSet);
			
			if (baseTileToSet is OsnowaTile osnowaTile)
			{
				_sceneContext.AllTilemapsByLayers[layerId].SetColor(position.ToVector3Int(), osnowaTile.Color);
			}
			else
			{
				_sceneContext.AllTilemapsByLayers[layerId].SetColor(position.ToVector3Int(), Color.white);
			}
			
			_positionFlagsResolver.SetFlagsAt(position.x, position.y, tilesByIdsCount, tilesByIds, 
				idsOfNotFoundTiles: null);
		}
		
		public void Set(Tilemap tilemap, Position position, OsnowaBaseTile baseTileToSet)
		{
			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds();
			int tilesByIdsCount = tilesByIds.Length;
			tilemap.SetTile(position.ToVector3Int(), baseTileToSet);
			
			if (baseTileToSet is OsnowaTile osnowaTile)
			{
				tilemap.SetColor(position.ToVector3Int(), osnowaTile.Color);
			}
			else
			{
				tilemap.SetColor(position.ToVector3Int(), Color.white);
			}
			
			_positionFlagsResolver.SetFlagsAt(position.x, position.y, tilesByIdsCount, tilesByIds, 
				idsOfNotFoundTiles: null);
		}

		/// <inheritdoc />
		public void SetMany(Tilemap tilemap, HashSet<Position> totalVisibleByThem, OsnowaBaseTile tilesetVisibleByOthers)
		{
			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds();
			int tilesByIdsCount = tilesByIds.Length;

			Vector3Int[] positions = totalVisibleByThem.Select(p => p.ToVector3Int()).ToArray();
			OsnowaBaseTile[] tiles = new OsnowaBaseTile[positions.Length];
			for (int i = 0; i < tiles.Length; i++)
			{
				tiles[i] = tilesetVisibleByOthers;
			}
			
			tilemap.SetTiles(positions, tiles);

			if (tilesetVisibleByOthers is OsnowaTile osnowaTile)
			{
				foreach (Position position in totalVisibleByThem)
				{
					tilemap.SetColor(position.ToVector3Int(), osnowaTile.Color);
				}
			}
			else
			{
				foreach (Position position in totalVisibleByThem)
				{
					tilemap.SetColor(position.ToVector3Int(), Color.white);
				}
			}

			foreach (Position position in totalVisibleByThem)
			{
				_positionFlagsResolver.SetFlagsAt(position.x, position.y, tilesByIdsCount, tilesByIds, idsOfNotFoundTiles: null);	
			}
		}

		public void RefreshTile(Position position, int layer)
		{
			var tileId = _tileMatricesByteByLayer[layer].Get(position);

			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds();

			OsnowaBaseTile baseTile = tilesByIds[tileId];

			_sceneContext.AllTilemapsByLayers[layer].SetTile(position.ToVector3Int(), baseTile);
		}

		/// <inheritdoc />
		public void ClearForTile(OsnowaBaseTile tileToCleanItsLayer)
		{
			int layerId = (int) tileToCleanItsLayer.Layer;
			_sceneContext.AllTilemapsByLayers[layerId].ClearAllTiles();
		}
	}
}