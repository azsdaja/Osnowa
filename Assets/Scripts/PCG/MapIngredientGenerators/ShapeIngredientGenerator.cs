namespace PCG.MapIngredientGenerators
{
	using System.Collections;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;
	using UnityEngine;

	public class ShapeIngredientGenerator : MapIngredientGenerator
	{
		private ShapeIngredientConfig _config;
		private ValueMap _initialShapeValues;
		private WorldGeneratorConfig _worldGeneratorConfig;

		public void Init(IExampleContext context, ShapeIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig, ValueMap initialShapeValues)
		{
			_worldGeneratorConfig = worldGeneratorConfig;
			_config = config;
			_initialShapeValues = initialShapeValues;
			base.Init(context, config, worldGeneratorConfig);

			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);
		}

		public override IEnumerator Recalculating()
		{
			OsnowaBaseTile dirtBaseTile = _worldGeneratorConfig.Tileset.DryDirt;
			MatrixByte dirtMatrixByte = GameContext.TileMatricesByLayer[dirtBaseTile.Layer];
			float seaLevel = GameContext.SeaLevel;
			foreach (Position position in Values.AllCellMiddles())
			{
				float value = _initialShapeValues.Get(position);
				bool isLand = value > seaLevel;
				value = isLand ? float.MaxValue : float.MinValue;
				Values.Set(position, value);
				if(isLand)
					dirtMatrixByte.Set(position, dirtBaseTile.Id);
			}

			yield return new WaitForSeconds(0.1f);
		}
	}
}