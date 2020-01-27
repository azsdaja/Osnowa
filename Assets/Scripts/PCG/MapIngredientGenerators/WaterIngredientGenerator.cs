using Position = Osnowa.Osnowa.Core.Position;

namespace PCG.MapIngredientGenerators
{
	using System.Collections;
	using System.Linq.Expressions;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Rng;
	using Osnowa.Osnowa.Tiles;
	using UnityEngine;

	public class WaterIngredientGenerator : MapIngredientGenerator
	{
		public const float Ground = 0.0f;
		public const float Sea = 0.15f;
		public const float River = 0.35f;
		private IRandomNumberGenerator _rng;
		private ValueMap _heightMapValues;
		private WaterIngredientConfig _config;
		private float _seaLevel;
		private int _riverPositions;
		private int _lakePositions;

		private Tileset _tileset;

		public void Init(IExampleContext context, WaterIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig, IRandomNumberGenerator rng, ValueMap heightMapValues)
		{
			_config = config;
			_heightMapValues = heightMapValues;
            base.Init(context, config, worldGeneratorConfig);

			_rng = rng;
			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);

			_tileset = worldGeneratorConfig.Tileset;
		}

		public override IEnumerator Recalculating()
		{
			_seaLevel = GameContext.SeaLevel;
			var matrixByteForWaterLayer = GameContext.TileMatricesByLayer[(int)TilemapLayer.Water];
			var matrixByteForDirtLayer = GameContext.TileMatricesByLayer[(int)TilemapLayer.Dirt];

			byte saltyWaterId = _tileset.SaltyWater.Id;
			byte dirtId = _tileset.DryDirt.Id;
			foreach (Position cellMiddle in Values.AllCellMiddles())
			{
                matrixByteForWaterLayer.Set(cellMiddle, saltyWaterId);

				if (_heightMapValues.Get(cellMiddle) < _seaLevel)
				{
					Values.Set(cellMiddle, Sea);
				}
                else
                {
                    matrixByteForDirtLayer.Set(cellMiddle, dirtId);
                    Values.Set(cellMiddle, Ground);
                }
            }

			float mapHectars = Values.XSize*Values.YSize/10000f;
			int lakeCount = (int)(mapHectars * _config.SoleLakesCountPerHectar);

			for (int i = 0; i < lakeCount; i++)
			{
				int lakePositions = (int) _config.RandomToSoleLakeArea.Evaluate(_rng.NextFloat());
                yield return new WaitForSeconds(0.1f);
			}

			yield return new WaitForSeconds(0.1f);
		}
    }
}