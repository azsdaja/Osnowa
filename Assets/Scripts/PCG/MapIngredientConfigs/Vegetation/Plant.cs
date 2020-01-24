namespace PCG.MapIngredientConfigs.Vegetation
{
	using System.Collections.Generic;
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Plant", menuName = "Kafelki/PCG/Vegetation/Plant", order = 0)]
	public class Plant : ScriptableObject
	{
        [Header("Unique value between 0 and 1 for this type of plant")]
		public float Value;
        public KafelkiTile Tile;

        [Header("Survival score modifiers")]
        public float ScoreOnWater = -1f;
        public float ScoreOnSand = 0.3f;
		public float ScoreOnSoil = 0.3f;
		public int SeedsSpread = 8;
		public int SeedsAmount = 5;

        [Header("Survival")]
        public float ScoreToDie = 0.3f;
        public float ScoreToSpreadSeeds = 1.0f;
        public int RootsRange = 2;
		public float NutritionTaken = 0f;
        public AnimationCurve NeighboursRatioToScore;
        public AnimationCurve HeightToScore;
        public List<Plant> LikedNeighbours;
        public List<Plant> DislikedNeighbours;

        [Header("Plant to replace this one when spreading or dying")]
        public Plant ChildWhenSpreading;
		public Plant ChildWhenDying;

        [Header("Special type of plant (like grass) that can grow at same position (but in different layer) as another plant")]
        public bool GrowsBelowOtherPlants;
		
	}
}