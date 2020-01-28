namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "WaterIngredientConfig", menuName = "Osnowa/PCG/Maps/WaterIngredientConfig", order = 0)]
	public class WaterIngredientConfig : MapIngredientConfig
	{
        [Range(1,15)]
		public int RiverThickness = 3;

		[Range(0f, 1.0f)]
		public float MinimalHeightAboveSeeToStartRiver = 0.1f;

		public bool Carve;

		[Range(0f, 0.05f)]
		public float DeltaToCarve;

		[Range(0f, 0.05f)]
		public float CarvingDepth;

		[Range(0.0f, 5f)]
		public float SoleLakesCountPerHectar = 0.2f;

		[Range(0f, 1f)]
		public float HeightScoreInfluenceForLakes = 0.5f;

		[Range(-0.1f, 0.1f)]
		public float MinHeightModifierToSeaLevel = 0f;

		public AnimationCurve RandomToSoleLakeArea;
		public AnimationCurve DistanceToRoundnessScoreForSoleLakes;
	}
}