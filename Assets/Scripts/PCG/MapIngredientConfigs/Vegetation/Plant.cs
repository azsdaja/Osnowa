namespace PCG.MapIngredientConfigs.Vegetation
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;
	using UnityEngine;
	using UnityEngine.Serialization;

	[CreateAssetMenu(fileName = "Plant", menuName = "Osnowa/PCG/Vegetation/Plant", order = 0)]
	public class Plant : ScriptableObject
	{
        [Header("Unique value between 0 and 1 for this type of plant")]
		public float Value;
        public OsnowaBaseTile Tile;

        [Header("If a plant scores below this value, it dies.")]
        public float ScoreToDie = 0.3f;
		
        [Header("If a plant scores below this value, it multiplies.")]
        public float ScoreToSpreadSeeds = 1.0f;
        
        [Header("Score factor from soil type")]
        public float ScoreOnWater = -1f;
        public float ScoreOnSand = 0.3f;
		public float ScoreOnSoil = 0.3f;
		
		[Header("Range of new plants spread when multiplying.")]
		public int SeedsSpread = 8;
		
		[Header("Amount of new plants spread when multiplying.")]
		public int SeedsAmount = 5;

		[Header("The more nutrition a plant takes, the more it prevents its neighbours in its roots range from growing.")]
        public int RootsRange = 2;
		public float NutritionTaken = 0f;
        [FormerlySerializedAs("NeighboursRatioToScore")] public AnimationCurve AvailableNutritionRatioToScore;
        
        [Header("Score factor taken from terrain height.")]
        public AnimationCurve HeightToScore;
        
        [Header("Plants that will improve this plant's score if in neighourhood.")]
        public List<Plant> LikedNeighbours;
        
        [Header("Plants that will decrease this plant's score if in neighourhood.")]
        public List<Plant> DislikedNeighbours;

        [Header("Plant to replace this one when spreading or dying")]
        public Plant ChildWhenSpreading;
		public Plant ChildWhenDying;

        [Header("Special type of plant (like grass) that can grow at same position (but in different layer) as another plant")]
        public bool GrowsBelowOtherPlants;
		
	}
}