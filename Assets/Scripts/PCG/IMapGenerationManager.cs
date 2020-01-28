namespace PCG
{
	using System.Collections;
	using MapIngredientGenerators;

	public interface IMapGenerationManager
	{
		IEnumerator CleanMaps();
		IEnumerator GenerateMapsAsync(bool skipImageBlending);

		MapIngredientGenerator CurrentlyGeneratedMap { get; }
	}
}