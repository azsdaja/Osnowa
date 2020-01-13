namespace PCG
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using MapIngredientGenerators;
	using UnityEngine;
	using Debug = UnityEngine.Debug;

	public class MapGenerationManager : IMapGenerationManager
	{
		private readonly IGameConfig _gameConfig;
		private readonly ResultMap _resultMap;
		private readonly IList<MapIngredientGenerator> _previewPresenters;
		private readonly Action<string> _progressAppender;

		private readonly IList<MapIngredientGenerator> _allMapPresenters;

		public MapGenerationManager(IList<MapIngredientGenerator> allMapPresenters, ResultMap resultMap, IList<MapIngredientGenerator> previewPresenters, 
			Action<string> progressAppender)
		{
			_allMapPresenters = allMapPresenters;
			_resultMap = resultMap;
			_previewPresenters = previewPresenters;
			_progressAppender = progressAppender;
		}

		public MapIngredientGenerator CurrentlyGeneratedMap { set; get; }

		public IEnumerator CleanMaps()
		{
			foreach (MapIngredientGenerator mapPresenter in _allMapPresenters)
			{
				mapPresenter.gameObject.SetActive(false);
				mapPresenter.CleanUpVisualisation();
			}
			_resultMap?.DestroyChildren();
			Debug.Log("cleand maps!");
			yield return new WaitForSeconds(0.1f);
		}

		public IEnumerator GenerateMapsAsync(bool skipImageBlending)
		{
			Stopwatch overallStopwatch = Stopwatch.StartNew();

			foreach (MapIngredientGenerator mapIngredientGenerator in _allMapPresenters)
			{
                bool ImageOfNewMapIsUnderConstruction() => CurrentlyGeneratedMap != null && CurrentlyGeneratedMap.Image.canvasRenderer.GetAlpha() < 1f;

                while (ImageOfNewMapIsUnderConstruction())
				{
					yield return new WaitForSeconds(0.1f);
				}

				Stopwatch stopwatch = Stopwatch.StartNew();
				CurrentlyGeneratedMap = mapIngredientGenerator;

				_progressAppender(mapIngredientGenerator.Config.TextForBuildingMap);
				yield return new WaitForSeconds(0.01f);

				IEnumerator recalculatingSteps = mapIngredientGenerator.CleanUpAndStartRecalculating();
				while (recalculatingSteps.MoveNext())
				{
					if (mapIngredientGenerator.Config.PresentationMode == MapIngredientPresentationMode.ShowAfterEachStep)
					{
						mapIngredientGenerator.RefreshVisualisation();
						mapIngredientGenerator.gameObject.SetActive(true);
					}
					yield return recalculatingSteps.Current;
				}

				if (mapIngredientGenerator.Config.PresentationMode == MapIngredientPresentationMode.BlendInAfterFinish)
				{
					mapIngredientGenerator.gameObject.SetActive(true);
					mapIngredientGenerator.RefreshVisualisation();

					if (!skipImageBlending)
						mapIngredientGenerator.StartBlendingIn();
				}

				Debug.Log(mapIngredientGenerator.GetType().Name + ": " + stopwatch.ElapsedMilliseconds + " ms.");
				if (mapIngredientGenerator.MapsToRefreshAfterCalculating != null)
				{
					foreach (MapIngredientGenerator toRefresh in mapIngredientGenerator.MapsToRefreshAfterCalculating)
					{
						toRefresh.RefreshVisualisation();
					}
				}
			}
			
			Debug.Log("Overall: " + overallStopwatch.ElapsedMilliseconds);

			_resultMap?.Recreate();

			CurrentlyGeneratedMap = null;

		}

		public Sprite GeneratePreview()
		{
			foreach (MapIngredientGenerator mapPresenter in _previewPresenters)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				CurrentlyGeneratedMap = mapPresenter;

				IEnumerator recalculatingSteps = mapPresenter.CleanUpAndStartRecalculating();
				while (recalculatingSteps.MoveNext())
				{
					//Debug.Log("kolejny krok");
				}
			}

			ValueMap previewMapValues = _previewPresenters.Last().Values;
			Sprite sprite = TextureGenerator.CreateSprite(previewMapValues);
			var blackToWhiteGradient = new Gradient{
				colorKeys =new[]
				{
					new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f) 
					
				}
			};
			TextureGenerator.ApplyValueMapToTexture(previewMapValues, blackToWhiteGradient, sprite.texture);
			//SaveSprite(sprite);
			return sprite;
		}

		public void GeneratePostPreview() // todo is it used at all?
		{
			IEnumerable<MapIngredientGenerator> postPreviewPresenters = _allMapPresenters.Where(p => !_previewPresenters.Contains(p));
			foreach (MapIngredientGenerator mapPresenter in postPreviewPresenters)
			{
				CurrentlyGeneratedMap = mapPresenter;

				IEnumerator recalculatingSteps = mapPresenter.CleanUpAndStartRecalculating();
				while (recalculatingSteps.MoveNext())
				{
					//Debug.Log("kolejny krok");
				}
			}

			ValueMap previewMapValues = _previewPresenters.Last().Values;
			Sprite sprite = TextureGenerator.CreateSprite(previewMapValues);
			var blackToWhiteGradient = new Gradient
			{
				colorKeys = new[]
				{
					new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f)

				}
			};
			TextureGenerator.ApplyValueMapToTexture(previewMapValues, blackToWhiteGradient, sprite.texture);
			//SaveSprite(sprite);
		}
    }
}