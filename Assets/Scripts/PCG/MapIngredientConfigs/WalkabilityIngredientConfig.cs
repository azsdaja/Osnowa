namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "WalkabilityIngredientConfig", menuName = "Kafelki/PCG/Maps/WalkabilityIngredientConfig", order = 0)]
	public class WalkabilityIngredientConfig : MapIngredientConfig
	{
		[Range(1, 25)]
		public int CellSize = 3;

		public AnimationCurve EvenessInfluence;

		public AnimationCurve HeightInfluence;

        [Range(0f, 1f)]
		public float WaterWalkabilityForced = 0.05f;
	}
}