namespace PCG.MapIngredientGenerators
{
	using System;
	using System.Collections;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Example;

	[Serializable]
	public class DebugIngredientGenerator : MapIngredientGenerator
	{
		public new void Init(IExampleContext context, MapIngredientConfig config, WorldGeneratorConfig worldGeneratorConfig)
		{
			Values = new ValueMap(1, worldGeneratorConfig.XSize, worldGeneratorConfig.YSize);

			base.Init(context, config, worldGeneratorConfig);
		}

		public override IEnumerator Recalculating()
		{
			foreach (Position position in Values.AllCellMiddles())
			{
				Values.Set(position, 0f);
			}
			yield break;
		}
	}
}