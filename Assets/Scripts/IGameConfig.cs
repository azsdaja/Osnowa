using Osnowa.NaPozniej;
using Osnowa.Osnowa.Entities;
using Osnowa.Osnowa.Example;
using Osnowa.Osnowa.Tiles;
using UnityEngine;

public interface IGameConfig
{
	ModeConfig ModeConfig { get; }
	Tileset Tileset { get; }
	int RngSeed { get; }
	EntityRecipees EntityRecipees { get; }
	ActorStatuses ActorStatuses { get; }
	Abilities Abilities { get; }

	int TurnsInDay { get; }
	Gradient TimeOfDayToColor { get; }
	Gradient HashToVisibleColor { get; }

	/// <summary> Expected format: ABCD, where AB is hours and CD is minutes. For example 2359 is 25:59. </summary>
	int DawnTriggerTime { get; }
	/// <summary> Expected format: ABCD, where AB is hours and CD is minutes. For example 2359 is 25:59. </summary>
	int DayTriggerTime { get; }
	/// <summary> Expected format: ABCD, where AB is hours and CD is minutes. For example 2359 is 25:59. </summary>
	int SunsetTriggerTime { get; }
	/// <summary> Expected format: ABCD, where AB is hours and CD is minutes. For example 2359 is 25:59. </summary>
	int NightTriggerTime { get; }

}