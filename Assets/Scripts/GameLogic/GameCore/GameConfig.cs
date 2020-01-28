namespace GameLogic.GameCore
{
	using Osnowa.NaPozniej;
	using Osnowa.Osnowa.Entities;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Tiles;
	using PCG;
	using UnityEngine;

	[CreateAssetMenu(fileName = "GameConfig", menuName = "Osnowa/Configuration/GameConfig", order = 0)]
	public class GameConfig : ScriptableObject, IGameConfig
	{
		[Header("RNG seed. Use 0 for random value.")]
		[SerializeField] private int _rngSeed = 1;

		public ModeConfig ModeConfig;
		public Tileset Tileset;
		public WorldGeneratorConfig WorldGeneratorConfig;
		public EntityRecipees EntityRecipees;
		public ActorStatuses ActorStatuses;
		public Abilities Abilities;
		public AnimationCurve TimeToBlinkingBrightness;
		public int TurnsInDay;
		public Gradient TimeOfDayToColor;
		public Gradient HashToVisibleColor;

		public float SuspiciousnessForLittleConcerned;
		public float SuspiciousnessForConcerned;
		public float SuspiciousnessForVeryConcerned;
		public float SuspiciousnessForAware;

		[Range(0, 2359)]
		public int DawnTrigger;
		[Range(0, 2359)]
		public int DayTrigger;
		[Range(0, 2359)]
		public int SunsetTrigger;
		[Range(0, 2359)]
		public int NightTrigger;

		public int RngSeed => _rngSeed;

		EntityRecipees IGameConfig.EntityRecipees => EntityRecipees;
		ActorStatuses IGameConfig.ActorStatuses => ActorStatuses;
		Abilities IGameConfig.Abilities => Abilities;
		
		int IGameConfig.TurnsInDay => TurnsInDay;
		Gradient IGameConfig.TimeOfDayToColor => TimeOfDayToColor;
		Gradient IGameConfig.HashToVisibleColor => HashToVisibleColor;

		int IGameConfig.DawnTriggerTime => DawnTrigger;
		int IGameConfig.DayTriggerTime => DayTrigger;
		int IGameConfig.SunsetTriggerTime => SunsetTrigger;
		int IGameConfig.NightTriggerTime => NightTrigger;

		ModeConfig IGameConfig.ModeConfig => ModeConfig;
		Tileset IGameConfig.Tileset => Tileset;
	}
}