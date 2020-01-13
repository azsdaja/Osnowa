namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "CivilizationIngredientConfig", menuName = "Kafelki/PCG/Maps/CivilizationIngredientConfig", order = 0)]
	public class CivilizationIngredientConfig : MoistureIngredientConfig
	{
		public AnimationCurve HeightInfluence;
		public AnimationCurve SweetWaterInfluence;
		public AnimationCurve SoilInfluence;
	}
}