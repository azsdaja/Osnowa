namespace GameLogic.AI.SkillEvaluation
{
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "FixedSkillEvaluator", menuName = "Kafelki/AI/SkillEvaluators/FixedSkillEvaluator", order = 0)]
	public class FixedSkillEvaluator : SkillEvaluator
	{
		[Range(0.0f, 1.0f)]
		public float ScoreToReturn;

		public override float EvaluateSkill(ISkillEvaluationContext context, Skill skill, GameEntity entity)
		{
			return ScoreToReturn;
		}
	}
}