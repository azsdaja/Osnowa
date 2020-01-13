namespace PCG
{
	using System.Collections.Generic;
	using System.Linq;
	using MapIngredientGenerators;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityUtilities;

	public class ResultMap : MonoBehaviour
	{
		public List<MapIngredientGenerator> MapsToShow;
		public List<float> Opacities;

		public void DestroyChildren()
		{
			foreach (GameObject child in transform.GetComponentsInChildren<Transform>()
				.Where(childTransform => childTransform != transform)
				.Select(t => t.gameObject))
			{
				DestroyImmediate(child);
			}
		}

		public void Recreate()
		{
			var parentRectTransform = GetComponent<RectTransform>();

			for (int index = 0; index < MapsToShow.Count; index++)
			{
				MapIngredientGenerator mapIngredientGenerator = MapsToShow[index];
				var mapObject = new GameObject("MapPart");
				mapObject.transform.parent = transform;
				mapObject.transform.localPosition = Vector3.zero;
				var image = mapObject.AddComponent<Image>();
				image.sprite = mapIngredientGenerator.GetComponent<Image>().sprite;
				var rectTransform = image.GetComponent<RectTransform>();
				rectTransform.SetWidth(parentRectTransform.GetWidth());
				rectTransform.SetHeight(parentRectTransform.GetHeight());
				Color newColor = new Color(image.color.r, image.color.g, image.color.b, Opacities[index]);
				image.color = newColor;
			}
		}
	}
}
