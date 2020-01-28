namespace GameLogic.AI.Model
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "Need", menuName = "Osnowa/AI/Needs/Need", order = 0)]
	public class Need : ScriptableObject
	{
		public string Name;

		public AnimationCurve SatisfactionToScore;
	}
}