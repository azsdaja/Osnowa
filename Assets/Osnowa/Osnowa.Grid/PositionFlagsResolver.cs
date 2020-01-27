namespace Osnowa.Osnowa.Grid
{
	using System.Collections.Generic;
	using System.Linq;
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using Core;
	using global::Osnowa.Osnowa.Context;
	using Unity.Tiles;

	public class PositionFlagsResolver : IPositionFlagsResolver
	{
		private readonly IGameConfig _gameConfig;
		private MatrixByte[] _tileMatricesByteByLayer;
		private PositionFlags _positionFlags;
		private readonly ILogger _logger;

		public PositionFlagsResolver(IOsnowaContextManager contextManager, IGameConfig gameConfig, ILogger logger)
		{
			contextManager.ContextReplaced += OnContextReplaced;
			_gameConfig = gameConfig;
			_logger = logger;
		}

		private void OnContextReplaced(IOsnowaContext newContext)
		{
			_positionFlags = newContext.PositionFlags;
			_tileMatricesByteByLayer = newContext.TileMatricesByLayer;
		}

		public void InitializePositionFlags()
		{
			int xSize = _tileMatricesByteByLayer.First().XSize;
			int ySize = _tileMatricesByteByLayer.First().YSize;
			var tileByIdProvider = new TileByIdProvider();
			OsnowaTile[] tilesByIds = tileByIdProvider.GetTilesByIds(_gameConfig.Tileset);
			int tilesByIdsCount = tilesByIds.Length;

			var idsOfNotFoundTiles = new List<int>();
			for (int x = 0; x < xSize; x++)
				for (int y = 0; y < ySize; y++)
				{
					SetFlagsAt(x, y, tilesByIdsCount, tilesByIds, idsOfNotFoundTiles);
				}

			if (idsOfNotFoundTiles.Any())
			{
				_logger.Warning("Not found tiles for ids: " + string.Join(", ", idsOfNotFoundTiles));
			}
			_logger.Info("initialized position flags");
		}

		public void SetFlagsAt(int x, int y, int tilesByIdsCount, OsnowaTile[] tilesByIds, List<int> idsOfNotFoundTiles)
		{
			bool isWalkable = true;
			bool isPassingLight = true;

			foreach (MatrixByte layerMatrix in _tileMatricesByteByLayer)
			{
				int tileId = layerMatrix.Get(x, y);
				if (tileId == 0)
					continue;
				OsnowaTile tile = null;
				if (tileId < tilesByIdsCount)
				{
					tile = tilesByIds[tileId];
				}
				if (tile == null)
				{
					idsOfNotFoundTiles?.Add(tileId);
					continue;
				}

				if (tile.Walkability == WalkabilityModifier.ForceWalkable)
					isWalkable = true;
				else if (tile.Walkability == WalkabilityModifier.ForceUnwalkable)
					isWalkable = false;

				if (tile.IsPassingLight == PassingLightModifier.ForcePassing)
					isPassingLight = true;
				else if (tile.IsPassingLight == PassingLightModifier.ForceBlocking)
					isPassingLight = false;
			}

			_positionFlags.SetFlags(x, y, isWalkable, isPassingLight);
		}
	}
}