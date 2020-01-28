using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Core;
using Osnowa.Osnowa.Tiles;
using Osnowa.Osnowa.Unity.Tiles;
using Osnowa.Osnowa.Unity.Tiles.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator
{
	private readonly ISceneContext _sceneContext;
	private readonly IOsnowaContextManager _contextManager;
	private Tilemap[] _allTilemaps;
	private readonly Tileset _tileset;
	private readonly TileByIdFromFolderProvider _tileByIdFromFolderProvider = new TileByIdFromFolderProvider();

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

		OsnowaBaseTile[] tilesByIds = _tileByIdFromFolderProvider.GetTilesByIds();

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

					OsnowaBaseTile baseTile = tilesByIds[tileId];
					if (baseTile == null)
						throw new System.Exception($"Tile with ID {tileId} not found in tileset, but placed on map.");
					PrepareTileToSet(x, y, batchPositionsLayers, batchTilesLayers, (TilemapLayer) matrixLayer, baseTile);

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
		OsnowaBaseTile[] fogOfWarToSet = Enumerable.Repeat(_tileset.FogOfWar, fogOfWarBounds.size.x * fogOfWarBounds.size.y).ToArray();
		_sceneContext.FogOfWarTilemap.SetTilesBlock(fogOfWarBounds, fogOfWarToSet);

		BoundsInt maskBounds = _sceneContext.TilemapDefiningOuterBounds.cellBounds;
		OsnowaBaseTile[] unseenMaskToSet = Enumerable.Repeat(_tileset.UnseenMask, maskBounds.size.x * maskBounds.size.y).ToArray();
		_sceneContext.UnseenMaskTilemap.SetTilesBlock(maskBounds, unseenMaskToSet);

		UnityEngine.Debug.Log("Tile generation time for " + xSize * ySize + " positions: " + stopwatch.ElapsedMilliseconds);
		UnityEngine.Debug.Log("Total tiles: " + batchPositionsLayers.Sum(l => l.Count));
	}

	private void CreateBatchPositionsAndTilesLayers(int totalMapArea, out List<Vector3Int>[] batchPositionsLayers, out List<TileBase>[] batchTilesLayers)
	{
		batchPositionsLayers = new List<Vector3Int>[(int)TilemapLayer.TotalLayersCount];
		batchTilesLayers = new List<TileBase>[(int)TilemapLayer.TotalLayersCount];

		// some approximations to save memory allocations for lists of positions
		const float approximatedFulfillmentForWater = 0.9f;
		const float approximatedFulfillmentForDirt = 0.7f;
		const float approximatedFulfillmentForSoil = 0.3f;
		const float approximatedFulfillmentForFloor = 0.25f;
		const float approximatedFulfillmentForRest = 0.01f;

		batchPositionsLayers[(int)TilemapLayer.Water] = new List<Vector3Int>((int)(totalMapArea * approximatedFulfillmentForWater));
		batchTilesLayers[(int)TilemapLayer.Water] = new List<TileBase>((int)(totalMapArea * approximatedFulfillmentForWater));
		batchPositionsLayers[(int)TilemapLayer.Dirt] = new List<Vector3Int>((int)(totalMapArea * approximatedFulfillmentForDirt));
		batchTilesLayers[(int)TilemapLayer.Dirt] = new List<TileBase>((int)(totalMapArea * approximatedFulfillmentForDirt));
		batchPositionsLayers[(int)TilemapLayer.Soil] = new List<Vector3Int>((int)(totalMapArea * approximatedFulfillmentForSoil));
		batchTilesLayers[(int)TilemapLayer.Soil] = new List<TileBase>((int)(totalMapArea * approximatedFulfillmentForSoil));
		batchPositionsLayers[(int)TilemapLayer.Floor] = new List<Vector3Int>((int)(totalMapArea * approximatedFulfillmentForFloor));
		batchTilesLayers[(int)TilemapLayer.Floor] = new List<TileBase>((int)(totalMapArea * approximatedFulfillmentForFloor));

		for (int i = (int)TilemapLayer.Floor + 1; i < (int)TilemapLayer.TotalLayersCount; i++)
		{
			batchPositionsLayers[i] = new List<Vector3Int>((int)(totalMapArea * approximatedFulfillmentForRest));
			batchTilesLayers[i] = new List<TileBase>((int)(totalMapArea * approximatedFulfillmentForRest));
		}
	}

	private void PrepareTileToSet(int x, int y, List<Vector3Int>[] batchPositionsLayers, List<TileBase>[] batchTilesLayers, TilemapLayer matrixLayer, OsnowaBaseTile baseTile)
	{
		if (matrixLayer != baseTile.Layer)
		{ 
			// todo: detect all occurences of situation below, but log once
			//Debug.LogWarning($"Layer mismatch on {x}, {y}: {tile.name} placed on layer {matrixLayer}, but tile layer is {tile.Layer}");
		}
		batchPositionsLayers[(int) matrixLayer].Add(new Vector3Int(x, y, 0));
		batchTilesLayers[(int) matrixLayer].Add(baseTile);
	}
}