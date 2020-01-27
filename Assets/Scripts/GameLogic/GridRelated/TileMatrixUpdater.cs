namespace GameLogic.GridRelated
{
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Unity.Tiles;
	using UnityUtilities;

	public class TileMatrixUpdater : ITileMatrixUpdater
	{
		private readonly IPositionFlagsResolver _positionFlagsResolver;
		private readonly IGameConfig _gameConfig;
		private readonly ITileByIdProvider _tileByIdProvider;
		private readonly ISceneContext _sceneContext;
		private OsnowaTile[] _tilesByIds;
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

		public void Set(Position position, OsnowaTile tileToSet)
		{
			_tileMatricesByteByLayer[tileToSet.Layer].Set(position, tileToSet.Id);

			OsnowaTile[] tilesByIds = _tileByIdProvider.GetTilesByIds(_gameConfig.Tileset);
			int tilesByIdsCount = tilesByIds.Length;

			_sceneContext.AllTilemapsByLayers[tileToSet.Layer].SetTile(position.ToVector3Int(), tileToSet);

			_positionFlagsResolver.SetFlagsAt(position.x, position.y, tilesByIdsCount, tilesByIds, 
				idsOfNotFoundTiles: null);
		}

		public void RefreshTile(Position position, int layer)
		{
			var tileId = _tileMatricesByteByLayer[layer].Get(position);

			if(_tilesByIds == null)
				_tilesByIds = _tileByIdProvider.GetTilesByIds(_gameConfig.Tileset);

			OsnowaTile tile = _tilesByIds[tileId];

			_sceneContext.AllTilemapsByLayers[layer].SetTile(position.ToVector3Int(), tile);
		}
	}
}