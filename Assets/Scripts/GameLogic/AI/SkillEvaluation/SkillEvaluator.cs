namespace GameLogic.AI.SkillEvaluation
{
	using Model;
	using UnityEngine;

	public abstract class SkillEvaluator : ScriptableObject
	{
		public abstract float EvaluateSkill(ISkillEvaluationContext context, Skill skill, GameEntity entity);
	}
}