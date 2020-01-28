namespace Osnowa.Osnowa.Example
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "ModeConfig", menuName = "Osnowa/Configuration/ModeConfig", order = 1)]
	public class ModeConfig : ScriptableObject
	{
		public bool ShowPaths;
		public bool ShowActorTooltip;
		public bool ShowPathRegister;
		public bool DebugActorUi;
		public Vision Vision;
	}

	public enum Vision
	{
		Undiscovered, Discovered, Permanent
	}
}