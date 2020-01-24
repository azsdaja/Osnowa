namespace PCG.MapIngredientGenerators
{
	using System.Collections;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Rng;
	using Osnowa.Osnowa.Tiles;
	using UnityEngine;

	public class SoilIngredientGenerator : MapIngredientGenerator
	{
		public const float None = 0.0f;

		public const float Soil = 0.15f;
        public const float Sand = 0.55f;

		private IRandomNumberGenerator _rng;
		private ValueMap _heightMapValues;
        private ValueMap _waterMapValues;
		private SoilIngredientConfig _config;

        private MatrixByte[] _tileMatricesByte;
		private Tileset _tileset;
        private float _dangerPerStepsToStart;

		public void Init(IExampleContext context, SoilIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig, 
            IRandomNumberGenerator rng, ValueMap heightMapValues, 
            ValueMap waterMapValues)
		{
            _heightMapValues = heightMapValues;
			_waterMapValues = waterMapValues;
			_config = config;
			base.Init(context, config, worldGeneratorConfig);

			_rng = rng;
			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);
			new Position(_rng.Next(10000), _rng.Next(10000));
			_tileMatricesByte = context.TileMatricesByteByLayer;
			_tileset = worldGeneratorConfig.Tileset;
		}

		public override IEnumerator Recalculating()
		{
			byte sandTileId = _tileset.Sand.Id;
			byte soilId = _tileset.Soil.Id;
            float seaLevel = GameContext.SeaLevel;

            int layer = TilemapLayers.Soil;

            foreach (Position position in Values.AllCellMiddles())
			{
				if (_waterMapValues.Get(position) != WaterIngredientGenerator.Ground)
				{
					Values.Set(position, None);
					continue;
				}

				float height = _heightMapValues.Get(position);

                float aboveSeaLevel = height - seaLevel;
                if (aboveSeaLevel < _config.HeightAboveSeaForNormalSoil)
                {
                    Values.Set(position, Sand);
                    _tileMatricesByte[layer].Set(position, sandTileId);
                }
                else
                {
                    Values.Set(position, Soil);
                    _tileMatricesByte[layer].Set(position, soilId);
                }
			}

			yield return new WaitForSeconds(0.1f);
		}
    }
}