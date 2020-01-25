namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "SlopinessIngredientConfig", menuName = "Osnowa/PCG/Maps/SlopinessIngredientConfig", order = 0)]
	public class SlopinessIngredientConfig : MapIngredientConfig
	{
		[Range(1,100)]
		public int ProbesPer8Direction = 10;

		[Range(1,5)]
		public int ProbeSpread = 3;

		[Range(1, 25)]
		public int CellSize = 5;
	}
}