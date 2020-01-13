#if UNITY_EDITOR
#endif

#if UNITY_EDITOR
#endif

namespace PCG.MapIngredientGenerators
{
	using System;
	using System.Collections;
	using Libraries.EditorCoroutines.Scripts;
	using MapIngredientConfigs;
	using Osnowa.Osnowa.Example;
	using UnityEngine;
	using UnityEngine.UI;

	[Serializable]
	public abstract class MapIngredientGenerator : MonoBehaviour
	{
		[SerializeField, HideInInspector] private Image _image;
		public MapIngredientGenerator[] MapsToRefreshAfterCalculating;
		protected IExampleContext GameContext;

		[HideInInspector] public ValueMap Values;

		public bool VisualisationUpToDate { get; set; }

		public Image Image => GetComponent<Image>();

		public MapIngredientConfig Config { get; private set; }

		public void CleanUpVisualisation()
		{
			VisualisationUpToDate = false;
			Image.sprite = null;
		}

		public void CleanUpData()
		{
			VisualisationUpToDate = false;
			Values.Clear();
		}

		public abstract IEnumerator Recalculating();

		public IEnumerator CleanUpAndStartRecalculating()
		{
			CleanUpData();
			return Recalculating();
		}

		public virtual void RefreshVisualisation()
		{
			if (Image.sprite == null)
			{
				Image.sprite = TextureGenerator.CreateSprite(Values);
			}

			TextureGenerator.ApplyValueMapToTexture(Values, Config.ValueToColor, Image.sprite.texture);

			VisualisationUpToDate = true;
		}

		public void ApplyTexture()
		{
			Image.sprite.texture.Apply();
		}

		protected void Init(IExampleContext context, MapIngredientConfig mapIngredientConfig, WorldGeneratorConfig worldGeneratorConfig)
		{
			GameContext = context;
			Config = mapIngredientConfig;

			Config.OnUpdate += () =>
			{
#if UNITY_EDITOR
				EditorCoroutines.StartCoroutine(CleanUpAndStartRecalculating(), this);
#endif
				RefreshVisualisation();
			};
		}

		public void StartBlendingIn()
		{
			Image.canvasRenderer.SetAlpha(0.0f);
			Image.CrossFadeAlpha(1f, 2f, true);
		}
	}
}
