namespace PCG.MapIngredientConfigs.Vegetation
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "VegetationIngredientConfig", menuName = "Kafelki/PCG/Maps/VegetationIngredientConfig", order = 0)]
	public class VegetationIngredientConfig : MapIngredientConfig
	{
		[Range(0, 1000)]
		public int Iterations = 100;

		[Range(0f, 1f)]
		public float InitialSeedsPerPosition = 0.05f;

		public Plant[] PlantDefinitions;
	}
}