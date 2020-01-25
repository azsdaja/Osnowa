namespace GameLogic.AI.Model
{
	using GameCore;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Ability", menuName = "Osnowa/Logic/Ability", order = 0)]
	public class Ability : ScriptableObject
	{
		public Sprite Sprite;
		[Header("Order should be unique, between 0 and 99.")]
		public int Order;
		public Color BackgroundColor;
		[Multiline]
		public string Description;

		public bool IsContextual;
		public KeyCode KeyCode;
		public KeyCode AlternativeKeyCode;
		public KeyCode AlternativeKeyCode2;
		public bool AllowRepeatingInput;
		public Decision Decision;
		public bool RequiresDirection;
		public bool RequiresPosition;
	}
}