namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "HeightIngredientConfig", menuName = "Kafelki/PCG/Maps/HeightIngredientConfig", order = 0)]
	public class HeightIngredientConfig : MapIngredientConfig
	{
		[Range(0f, 0.5f)]
		public float ProbeMaxReachAsSizePercentage = 0.3f;

		[Range(1, 50)]
		public int ProbeCount = 15;

		[Range(0.1f, 1.0f)]
		public float GroundPercentageInfluence = 0.5f;
	}
}