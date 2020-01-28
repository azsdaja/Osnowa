namespace PCG
{
	using GameLogic.GameCore;
	using MapIngredientConfigs;
	using MapIngredientConfigs.Vegetation;
	using Osnowa.Osnowa.Tiles;
	using UnityEngine;

	[CreateAssetMenu(fileName = "WorldGeneratorConfig", menuName = "Osnowa/PCG/WorldGeneratorConfig", order = 0)]
	public class WorldGeneratorConfig : ScriptableObject
	{
		public InitialHeightIngredientConfig InitialHeightIngredientConfig;
		public PersistenceInputIngredientConfig PersistenceInputIngredientConfig;
		public ShapeIngredientConfig ShapeIngredientConfig;
		public HeightIngredientConfig HeightIngredientConfig;
		public WaterIngredientConfig WaterIngredientConfig;
		public MoistureIngredientConfig MoistureIngredientConfig;
		public SoilIngredientConfig SoilIngredientConfig;
		public SlopinessIngredientConfig SlopinessIngredientConfig;
		public CivilizationIngredientConfig CivilizationIngredientConfig;
		public BuildingIngredientConfig BuildingIngredientConfig;
		public WalkabilityIngredientConfig WalkabilityIngredientConfig;
		public VegetationIngredientConfig VegetationIngredientConfig;
		public MapIngredientConfig DebugMapIngredientConfig;
		public Tileset Tileset;
		public GameConfig GameConfig;

		public GameObject MapPresenterPrefab;
		public int XSize;
		public int YSize;
	}
}