namespace PCG.MapIngredientGenerators
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using FloodSpill;
	using GameLogic.GameCore;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.CSharpUtilities;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Pathfinding;
	using Osnowa.Osnowa.Rng;
	using Osnowa.Osnowa.Tiles;
	using UnityEngine;
	using Bounds = Osnowa.Osnowa.Core.Bounds;
	using Grid = Osnowa.Osnowa.Grid.Grid;
	using Position = Osnowa.Osnowa.Core.Position;

	public class BuildingIngredientGenerator : MapIngredientGenerator
	{
		public static float Nothing = 0.05f;
        public static float House = 0.25f;
		public static float Field = 0.35f;
		public static float Road = 0.45f;

        private ValueMap _soilMap;
		private MatrixFloat _walkability;
		private ValueMap _vegetationMap;
 
		private BuildingIngredientConfig _config;

		private IRandomNumberGenerator _rng;
		private Pathfinder _pathfinder;
		private MatrixByte[] _tileMatricesByte;

		private Tileset _tileset;
		private GameConfig _gameConfig;
		private Grid _grid;

		public void Init(IExampleContextManager contextManager, BuildingIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig, IRandomNumberGenerator rng,
			ValueMap civilizationMap, ValueMap soilMap, ValueMap vegetationMapValues, ValueMap debugMapValues)
		{
			_rng = rng;
			_config = config;
            _soilMap = soilMap;
			_walkability = contextManager.Current.Walkability;
			_vegetationMap = vegetationMapValues;
			base.Init(contextManager.Current, config, worldGeneratorConfig);

			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);
            _tileMatricesByte = GameContext.TileMatricesByLayer;
			_tileset = worldGeneratorConfig.Tileset;
			_gameConfig = worldGeneratorConfig.GameConfig;
			
			_grid = new Grid(contextManager, true);
			_pathfinder = Pathfinder.Create(contextManager);
		}

		public override IEnumerator Recalculating() 
		{
			//_pathfinder.InitializeNavigationGrid(false);
			int hectarsInMap = Values.XSize*Values.YSize/10000;
			int housesToGenerate = (int) Math.Max(2f, _config.VillagesPerHectar*hectarsInMap);

			for (int i = 1; i <= housesToGenerate; i++)
            {
                List<Position> placeForHouse = RepeatedActionExecutor.Execute(FindPlaceForHouse, 1);
                Area area = new Area(placeForHouse);
                //BuildHouse(area);
				yield return new WaitForSeconds(0.1f);
			}
            
#if UNITY_EDITOR
			// osnowatodo EditorUtility.SetDirty(GameContext as ExampleContext);
#endif
		}

        private List<Position> FindPlaceForHouse()
        {
            var spiller = new FloodSpill.FloodSpiller();

            int[,] markMatrix = new int[Values.XSize, Values.YSize];
            var startingPoint = _rng.NextPosition(new Bounds());
            FloodParameters parameters = new FloodParameters(Values.XSize/2, Values.YSize/2)
            {
                BoundsRestriction = new FloodBounds(0, 0, Values.XSize, Values.YSize)
            };
            spiller.SpillFlood(parameters, markMatrix);

            return new List<Position>{new Position(50, 50)};
        }

        private Position BuildHouse(Area area)
		{
            bool ValidForBeingDoor(Position position) 
                => position.y == area.Bounds.yMin && position.x > area.Bounds.xMin && position.x < area.Bounds.xMax - 1;

            Position doorPosition = RepeatedActionExecutor.Execute(() =>
            {
                var position = _rng.Choice(area.Perimeter);
                return (ValidForBeingDoor(position), position);
            });
            
			foreach (Position position in area.Perimeter)
			{
				if (position != doorPosition)
				{
					Construct(position, House, _tileset.Wall);
				}
				else
				{
					Construct(position, -1f, null);
				}
			}

			KafelkiTile roofTile = _tileset.Roof;
			foreach (Position housePosition in area.Positions)
			{
				Construct(housePosition, -1f, roofTile);
				if (housePosition != doorPosition)
				{
					_grid.SetWalkability(housePosition, 0f);
					_walkability.Set(housePosition, 0f);
				}
			}

			BoundsInt boundsForRoofAdditions = area.Bounds;
			++boundsForRoofAdditions.xMin;
			++boundsForRoofAdditions.yMin;
			--boundsForRoofAdditions.xMax;
			--boundsForRoofAdditions.yMax;

			return doorPosition;
		}

		private void Construct(Position position, float buildingValue, KafelkiTile buildingTile)
		{
			if(buildingValue >= 0)
				Values.Set(position, buildingValue);
			//if(!new[]{ _tileset.Wall.Id, _tileset.DoorHorizontalClosed.Id}.Contains(_tileMatricesByte[TilemapLayers.Standing].Get(position)))
			//	_tileMatricesByte[TilemapLayers.Standing].Set(position, 0);
			_tileMatricesByte[TilemapLayers.Decoration].Set(position, 0);
			_tileMatricesByte[buildingTile.Layer].Set(position, buildingTile.Id);
		}

		private bool IsWater(Position position)
		{
			return _tileMatricesByte[TilemapLayers.Soil].Get(position) == 0;
		}
	}
}