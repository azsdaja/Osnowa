namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "InitialHeightIngredientConfig", menuName = "Kafelki/PCG/Maps/InitialHeightIngredientConfig", order = 0)]
	public class InitialHeightIngredientConfig : MapIngredientConfig
	{
		[Range(0.001f, 0.1f)]
		public float Scale = 0.03f;

		[Range(1, 4)]
		public int Octaves = 3;

		[Range(0.1f, 0.9f)]
		public float GroundPercentage = 0.5f;

		public AnimationCurve MaxFactorDistanceFromCenterToFalloff;
	}
}