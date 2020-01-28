namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = nameof(SoilIngredientConfig), menuName = "Osnowa/PCG/Maps/SoilIngredientConfig", order = 0)]
	public class SoilIngredientConfig : MapIngredientConfig
	{
        [Range(0.0f, 2f)]
		public float HeightAboveSeaForNormalSoil = 0.1f;
	}
}