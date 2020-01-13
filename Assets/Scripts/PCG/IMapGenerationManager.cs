namespace PCG
{
	using System.Collections;
	using MapIngredientGenerators;
	using UnityEngine;

	public interface IMapGenerationManager
	{
		IEnumerator CleanMaps();
		IEnumerator GenerateMapsAsync(bool skipImageBlending);

		MapIngredientGenerator CurrentlyGeneratedMap { get; }
		Sprite GeneratePreview();
		void GeneratePostPreview();
	}
}