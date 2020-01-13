namespace GameLogic.AI.Model
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "Need", menuName = "Kafelki/AI/Needs/Need", order = 0)]
	public class Need : ScriptableObject
	{
		public string Name;

		public AnimationCurve SatisfactionToScore;
	}
}