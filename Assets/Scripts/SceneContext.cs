using System;
using System.Collections.Generic;
using Cinemachine;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityUtilities;

public class SceneContext : MonoBehaviour, ISceneContext
{
	public UnityEngine.Grid GameGrid;
	public Light SkyLight;
	public Transform EffectTilemapsParent;
	public Transform EntitiesParent;
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable 649
	[SerializeField] private Tilemap _tilemapDefiningOuterBounds;
	[SerializeField] private Tilemap _waterTilemap;
	[SerializeField] private Tilemap _fogOfWarTilemap;
	[SerializeField] private Tilemap _unseenMaskTilemap;
	[SerializeField] private Tilemap _dirtTilemap;
	[SerializeField] private Tilemap _soilTilemap;
	[SerializeField] private Tilemap _floorsTilemap;
	[SerializeField] private Tilemap _standingTilemap;
	[SerializeField] private Tilemap _decorationsTilemap;
	[SerializeField] private Tilemap _roofTilemap;
	[SerializeField] private Tilemap _onRoofTilemap;
	[SerializeField] private Tilemap _debug1Tilemap;
	[SerializeField] private Tilemap _debug2Tilemap;
	[SerializeField] private Tilemap _debug3Tilemap;

#pragma warning restore IDE0044 // Add readonly modifier

#pragma warning restore 649

	public List<Tilemap> AllPresentableTilemaps;
	private HashSet<Position> _visiblePositions = new HashSet<Position>();
	[NonSerialized] private Tilemap[] _allTilemapsByLayers;
	[SerializeField] private CameraMouseControl _cameraMouseControl;
	[SerializeField] private CinemachineVirtualCamera _followPlayerCamera;

	UnityEngine.Grid ISceneContext.GameGrid => GameGrid;

	public Tilemap TilemapDefiningOuterBounds => _tilemapDefiningOuterBounds;
	public Tilemap FogOfWarTilemap => _fogOfWarTilemap;
	public Tilemap UnseenMaskTilemap => _unseenMaskTilemap;
	public Tilemap WaterTilemap => _waterTilemap;
	public Tilemap DirtTilemap => _dirtTilemap;
	public CameraMouseControl CameraMouseControl => _cameraMouseControl;
	public CinemachineVirtualCamera FollowPlayerCamera => _followPlayerCamera;
	public Tilemap SoilTilemap => _soilTilemap;
	public Tilemap FloorsTilemap => _floorsTilemap;
	public Tilemap StandingTilemap => _standingTilemap;
	public Tilemap DecorationsTilemap => _decorationsTilemap;
	public Tilemap RoofTilemap => _roofTilemap;
	public Tilemap OnRoofTilemap => _onRoofTilemap;
	public Tilemap Debug1Tilemap => _debug1Tilemap;
	public Tilemap Debug2Tilemap => _debug2Tilemap;
	public Tilemap Debug3Tilemap => _debug3Tilemap;

	Light ISceneContext.SkyLight => SkyLight;

	Transform ISceneContext.EffectTilemapsParent => EffectTilemapsParent;

	Transform ISceneContext.EntitiesParent => EntitiesParent;

	IList<Tilemap> ISceneContext.AllPresentableTilemaps => AllPresentableTilemaps;

	public IList<Tilemap> AllTilemapsByLayers
	{
		get
		{
			if (_allTilemapsByLayers == null)
				CalculateAllTilemapsByLayers();
			return _allTilemapsByLayers;
		}
	}

	private void CalculateAllTilemapsByLayers()
	{
		_allTilemapsByLayers = new Tilemap[(int)TilemapLayer.TotalLayersCount];
		_allTilemapsByLayers[(int)TilemapLayer.Water] = WaterTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Dirt] = DirtTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Soil] = SoilTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Floor] = FloorsTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Standing] = StandingTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Decoration] = DecorationsTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Roof] = RoofTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.OnRoof] = OnRoofTilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Debug2] = Debug1Tilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Debug1] = Debug2Tilemap;
		_allTilemapsByLayers[(int)TilemapLayer.Debug3] = Debug3Tilemap;
	}

	public HashSet<Position> VisiblePositions
	{
		get { return _visiblePositions; }
		set { _visiblePositions = value; }
	}
}