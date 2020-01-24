namespace PCG
{
	using System;
	using System.Collections;
	using MapIngredientGenerators;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Rng;
	using TMPro;
	using UnityEngine;
	using Zenject;

	public class MapGenerator : MonoBehaviour
	{
		private IMapGenerationManager _mapGenerationManager;
		private IRandomNumberGenerator _rng;
		private IExampleContextManager _contextManager;
		private IOsnowaContext _osnowaContext;

		public TextMeshProUGUI GeneratingText;
		public RectTransform MapFrame;

		public WorldGeneratorConfig WorldGeneratorConfig;

		public InitialHeightIngredientGenerator InitialHeightMap;
		public WaterIngredientGenerator WaterMap;
		public SoilIngredientGenerator Soil;
		public BuildingIngredientGenerator BuildingMap;
		public WalkabilityIngredientGenerator WalkabilityMap;
		public VegetationMapIngredientGenerator VegetationMap;
		public DebugIngredientGenerator DebugMap;
		public MapIngredientGenerator[] AllMapIngredientGeneratorsInCreationOrder;
		public MapIngredientGenerator[] PreviewIngredientsInCreationOrder;
		public bool SkipImageBlending;

		// runtime only!
		[Inject]
		public void Init(IRandomNumberGenerator rng, IExampleContextManager contextManager)
		{
			_rng = rng;
			_contextManager = contextManager;
		}

		public Sprite GeneratePreview()
		{
			_mapGenerationManager = CreateMapGenerationManager(AppendProgressToGenerationLog);
			Sprite texture = _mapGenerationManager.GeneratePreview();
			return texture;
		}

		public void GenerateAllAsync()
		{
			StartCoroutine(GeneratingInBackground(AppendProgressToGenerationLog));
		}

		public IEnumerator GeneratingInBackground(Action<string> progressAppender)
		{
			_mapGenerationManager = CreateMapGenerationManager(progressAppender);

			IEnumerator cleanMaps = _mapGenerationManager.CleanMaps();
			while (cleanMaps.MoveNext())
			{
				yield return new WaitForSeconds(0f);
			}

			IEnumerator generateMapsAsync = _mapGenerationManager.GenerateMapsAsync(SkipImageBlending);
			while (generateMapsAsync.MoveNext())
			{
				yield return new WaitForSeconds(0.1f);
			}
		}

		private IMapGenerationManager CreateMapGenerationManager(Action<string> progressAppender)
		{
			InitializeMaps(_rng);
			return new MapGenerationManager(AllMapIngredientGeneratorsInCreationOrder, null, PreviewIngredientsInCreationOrder,
				progressAppender);
		}

		private void InitializeMaps(IRandomNumberGenerator rng)
		{
			DebugMap.Init(_contextManager.Current, WorldGeneratorConfig.DebugMapIngredientConfig, WorldGeneratorConfig);
			InitialHeightMap.Init(_contextManager.Current, WorldGeneratorConfig.InitialHeightIngredientConfig, WorldGeneratorConfig, rng);
			WaterMap.Init(_contextManager.Current, WorldGeneratorConfig.WaterIngredientConfig, WorldGeneratorConfig, rng, InitialHeightMap.Values);
			Soil.Init(_contextManager.Current, WorldGeneratorConfig.SoilIngredientConfig, WorldGeneratorConfig, rng, InitialHeightMap.Values, WaterMap.Values);
			VegetationMap.Init(_contextManager.Current, WorldGeneratorConfig.VegetationIngredientConfig, WorldGeneratorConfig, rng, Soil.Values, InitialHeightMap.Values);
			WalkabilityMap.Init(_contextManager.Current, WorldGeneratorConfig.WalkabilityIngredientConfig, WorldGeneratorConfig, WaterMap.Values);
			BuildingMap.Init(_contextManager, WorldGeneratorConfig.BuildingIngredientConfig, WorldGeneratorConfig, rng, null,
				Soil.Values, VegetationMap.Values, DebugMap.Values);
		}

		private void AppendProgressToGenerationLog(string text)
		{
			GeneratingText.text += Environment.NewLine + text;
		}
	}
}