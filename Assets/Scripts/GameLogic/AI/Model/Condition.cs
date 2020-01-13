namespace GameLogic.AI.Model
{
	using Conditions;
	using UnityEngine;

	public abstract class Condition : ScriptableObject
	{
		public abstract bool Evaluate(GameEntity entity, IConditionContext conditionContext);
	}
}