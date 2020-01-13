namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "MoistureIngredientConfig", menuName = "Kafelki/PCG/Maps/MoistureIngredientConfig", order = 0)]
	public class MoistureIngredientConfig : MapIngredientConfig
	{
		[Range(1,100)]
		public int ProbesPer8Direction = 10;

		[Range(1,5)]
		public int ProbeSpread = 3;

		[Range(1, 25)]
		public int CellSize = 5;
	}
}