using System.Collections.Generic;
using Osnowa.Osnowa.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityUtilities;

/// <summary>
/// Collects data of the game state that can be accessed from the game logic.
/// </summary>
public interface ISceneContext
{
	/// <summary>
	/// Source of light from sky during day and night.
	/// </summary>
	Light SkyLight { get; } // todo use PlayerUiController?

	/// <summary>
	/// Unity Transform that should be the parent of all effect tilemaps.
	/// </summary>
	Transform EffectTilemapsParent { get; }

	/// <summary>
	/// Unity Transform that should be the parent of all the entities.
	/// </summary>
	Transform EntitiesParent { get; }
        
	/// <summary>
	/// Unity grid representing the world.
	/// </summary>
	UnityEngine.Grid GameGrid { get; }

	/// <summary>
	/// The size of this tilemap may (but doesn't need to) be used by the game logic (e.g. by the world generator) 
	/// to define the size of the world.
	/// </summary>
	Tilemap TilemapDefiningOuterBounds { get; }

	/// <summary>
	/// Tilemap for displaying black fog-of-war tiles.
	/// </summary>
	Tilemap FogOfWarTilemap { get; }

	/// <summary>
	/// Tilemap for displaying semi-transparent tiles to mask positions not seen but discovered.
	/// </summary>
	Tilemap UnseenMaskTilemap { get; }

	/// <summary>
	/// Tilemap for the bottom-most layer of the world.
	/// </summary>
	Tilemap WaterTilemap { get; }

	/// <summary>
	/// Tilemap for floors like grass, wooden floor, road, carpet.
	/// </summary>
	Tilemap DirtTilemap { get; }

	/// <summary>
	/// Tilemap for floors like grass, wooden floor, road, carpet.
	/// </summary>
	Tilemap FloorsTilemap { get; }

	/// <summary>
	/// Tilemap for standing objects like bushes, barrels, walls, trees, chairs.
	/// </summary>
	Tilemap StandingTilemap { get; }

	/// <summary>
	/// Tilemap for decorations (both for walls and floors).
	/// </summary>
	Tilemap DecorationsTilemap { get; }

	/// <summary>
	/// Collects all the tilemaps that should be manipulated when the field of view changes.
	/// </summary>
	IList<Tilemap> AllPresentableTilemaps { get; }

	/// <summary>
	/// All the tilemaps ordered by their layers.
	/// </summary>
	IList<Tilemap> AllTilemapsByLayers { get; }

	/// <summary>
	/// Collection of positions that are visible by the actor controlled by player.
	/// </summary>
	HashSet<Position> VisiblePositions { get; set; }

	CameraMouseControl CameraMouseControl { get; }

	Tilemap SoilTilemap { get; }
	Tilemap RoofTilemap { get; }
	Tilemap OnRoofTilemap { get; }
	Tilemap Debug1Tilemap { get; }
	Tilemap Debug2Tilemap { get; }
	Tilemap Debug3Tilemap { get; }
}