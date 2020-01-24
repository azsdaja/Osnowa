using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Core;
using Osnowa.Osnowa.Tiles;
using Osnowa.Osnowa.Unity.Tiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator
{
	private readonly ISceneContext _sceneContext;
	private readonly IOsnowaContextManager _contextManager;
	private Tilemap[] _allTilemaps;
	private readonly Tileset _tileset;
	private readonly TileByIdProvider _tileByIdProvider = new TileByIdProvider();

	public TilemapGenerator(ISceneContext sceneContext, IGameConfig gameConfig, IOsnowaContextManager contextManager)
	{
		_sceneContext = sceneContext;
		_contextManager = contextManager;
		_tileset = gameConfig.Tileset;
	}

	public void Clear()
	{
		foreach (Tilemap tilemap in _sceneContext.AllPresentableTilemaps)
		{
			tilemap.ClearAllTiles();
		}
		_sceneContext.FogOfWarTilemap.ClearAllTiles();
		_sceneContext.UnseenMaskTilemap.ClearAllTiles();
#if UNITY_EDITOR
		EditorUtility.SetDirty(_sceneContext.TilemapDefiningOuterBounds); // to make the scene dirty
#endif
	}

	public void Generate()
	{
		var stopwatch = Stopwatch.StartNew();
		IOsnowaContext context = _contextManager.Current;
		int xSize = context.PositionFlags.XSize;
		int ySize = context.PositionFlags.YSize;

		_allTilemaps = _sceneContext.AllTilemapsByLayers.ToArray();

		foreach (Tilemap tilemap in _sceneContext.AllPresentableTilemaps)
		{
			tilemap.ClearAllTiles();
		}

		int totalMapArea = xSize * ySize;

		List<Vector3Int>[] batchPositionsLayers;
		List<TileBase>[] batchTilesLayers;
		CreateBatchPositionsAndTilesLayers(totalMapArea, out batchPositionsLayers, out batchTilesLayers);

		KafelkiTile[] tilesByIds = _tileByIdProvider.GetTilesByIds(_tileset);

		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int matrixLayer = 0; matrixLayer < context.TileMatricesByLayer.Length; matrixLayer++)
				{
//						if (matrixLayer == TilemapLayers.Roof || matrixLayer == TilemapLayers.OnRoof)
//							continue;
					MatrixByte tileMatrixByte = context.TileMatricesByLayer[matrixLayer];
					byte tileId = tileMatrixByte.Get(x, y);
					if (tileId == 0) continue;

					KafelkiTile tile = tilesByIds[tileId];
					if (tile == null)
						throw new System.Exception($"Tile with ID {tileId} not found in tileset, but placed on map.");
					PrepareTileToSet(x, y, batchPositionsLayers, batchTilesLayers, matrixLayer, tile);

				}
			}
		}

		var stopwatchBatch = Stopwatch.StartNew();
		for (int tilemapIndex = 0; tilemapIndex < _allTilemaps.Length; tilemapIndex++)
		{
			Tilemap tilemap = _allTilemaps[tilemapIndex];
			tilemap.SetTiles(batchPositionsLayers[tilemapIndex].ToArray(), batchTilesLayers[tilemapIndex].ToArray());
			UnityEngine.Debug.Log(tilemap.name + " tilemap: " + stopwatchBatch.ElapsedMilliseconds + " milliseconds.");
			stopwatchBatch.Restart();
		}

		BoundsInt fogOfWarBounds = _sceneContext.TilemapDefiningOuterBounds.cellBounds;
		KafelkiTile[] fogOfWarToSet = Enumerable.Repeat(_tileset.FogOfWar, fogOfWarBounds.size.x * fogOfWarBounds.size.y).ToArray();
		_sceneContext.FogOfWarTilemap.SetTilesBlock(fogOfWarBounds, fogOfWarToSet);

		BoundsInt maskBounds = _sceneContext.TilemapDefiningOuterBounds.cellBounds;
		KafelkiTile[] unseenMaskToSet = Enumerable.Repeat(_tileset.UnseenMask, maskBounds.size.x * maskBounds.size.y).ToArray();
		_sceneContext.UnseenMaskTilemap.SetTilesBlock(maskBounds, unseenMaskToSet);

		UnityEngine.Debug.Log("Tile generation time for " + xSize * ySize + " positions: " + stopwatch.ElapsedMilliseconds);
		UnityEngine.Debug.Log("Total tiles: " + batchPositionsLayers.Sum(l => l.Count));
	}

	private void CreateBatchPositionsAndTilesLayers(int totalMapArea, out List<Vector3Int>[] batchPositionsLayers, out List<TileBase>[] batchTilesLayers)
	{
		batchPositionsLayers = new List<Vector3Int>[TilemapLayers.TotalLayersCount];
		batchTilesLayers = new List<TileBase>[TilemapLayers.TotalLayersCount];

		// some approximations to save memory allocations for lists of positions
		const float approximatedWaterFulfillment = 0.9f;
		const float approximatedDirtFulfillment = 0.7f;
		const float approximatedSoilFulfillment = 0.3f;
		const float approximatedFloorFulfillment = 0.25f;
		const float approximatedOverallFulfillment = 0.01f;

		batchPositionsLayers[TilemapLayers.Water] = new List<Vector3Int>((int)(totalMapArea * approximatedWaterFulfillment));
		batchTilesLayers[TilemapLayers.Water] = new List<TileBase>((int)(totalMapArea * approximatedWaterFulfillment));
		batchPositionsLayers[TilemapLayers.Dirt] = new List<Vector3Int>((int)(totalMapArea * approximatedDirtFulfillment));
		batchTilesLayers[TilemapLayers.Dirt] = new List<TileBase>((int)(totalMapArea * approximatedDirtFulfillment));
		batchPositionsLayers[TilemapLayers.Soil] = new List<Vector3Int>((int)(totalMapArea * approximatedSoilFulfillment));
		batchTilesLayers[TilemapLayers.Soil] = new List<TileBase>((int)(totalMapArea * approximatedSoilFulfillment));
		batchPositionsLayers[TilemapLayers.Floor] = new List<Vector3Int>((int)(totalMapArea * approximatedFloorFulfillment));
		batchTilesLayers[TilemapLayers.Floor] = new List<TileBase>((int)(totalMapArea * approximatedFloorFulfillment));

		for (int i = TilemapLayers.Floor + 1; i < TilemapLayers.TotalLayersCount; i++)
		{
			batchPositionsLayers[i] = new List<Vector3Int>((int)(totalMapArea * approximatedOverallFulfillment));
			batchTilesLayers[i] = new List<TileBase>((int)(totalMapArea * approximatedOverallFulfillment));
		}
	}

	private void PrepareTileToSet(int x, int y, List<Vector3Int>[] batchPositionsLayers, List<TileBase>[] batchTilesLayers, int matrixLayer, KafelkiTile tile)
	{
		if (matrixLayer != tile.Layer)
		{ 
			// todo: detect all occurences of situation below, but log once
			//Debug.LogWarning($"Layer mismatch on {x}, {y}: {tile.name} placed on layer {matrixLayer}, but tile layer is {tile.Layer}");
		}
		batchPositionsLayers[matrixLayer].Add(new Vector3Int(x, y, 0));
		batchTilesLayers[matrixLayer].Add(tile);
	}
}