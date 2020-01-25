namespace PCG.MapIngredientConfigs
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "BuildingIngredientConfig", menuName = "Osnowa/PCG/Maps/BuildingIngredientConfig", order = 0)]
	public class BuildingIngredientConfig : MapIngredientConfig
	{
		[Range(0.0f, 1.0f)]
		public float VillagesPerHectar = 0.2f;

		public AnimationCurve VillageAreaDistribution;

		public AnimationCurve CivilizationToSettlementChance;

		public AnimationCurve BuildingCountToStakewallChance;

		[Range(0, 100)]
		public int PreferredMinimalDistanceBetweenGroups = 40;

		[Range(0.3f, 2.0f)]
		public float FieldsPerHouse = 0.5f;

		[Range(1, 40)]
		public int FieldsMinDistanceFromVillageCenter = 10;

		[Range(5, 50)]
		public int FieldsMaxDistanceFromVillageCenter = 30;

		[Range(1, 10)]
		public int FieldsMinSize = 7;

		[Range(1, 20)]
		public int FieldsMaxSize = 12;

		public bool AdjustWalkabilityOnRoads = true;

		[Range(0f, 1f)]
		public float PlantSurvivalChanceInVillage = 0.2f;

		public AnimationCurve VillageAreaToHouseBuildingAttempts;
	}
}