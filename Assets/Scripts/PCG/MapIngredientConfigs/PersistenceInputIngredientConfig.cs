namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "PersistenceInputIngredientConfig", menuName = "Kafelki/PCG/Maps/PersistenceInputIngredientConfig", order = 0)]
	public class PersistenceInputIngredientConfig : MapIngredientConfig
	{
		[Range(0.001f, 0.1f)]
		public float Scale = 0.03f;

		[Range(1, 4)]
		public int Octaves = 3;

		[Range(0.1f, 1.0f)]
		public float Persistence = 0.5f;
	}
}