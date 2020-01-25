namespace GameLogic.AI.Conditions
{
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Attitude", menuName = "Osnowa/AI/Conditions/Attitude", order = 0)]
	public class Attitude : Condition
	{
		public bool Aggressive;

		public override bool Evaluate(GameEntity entity, IConditionContext conditionContext)
		{
			return entity.isAggressive == Aggressive;
		}
	}
}