namespace GameLogic.AI.SkillEvaluation
{
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "RandomSkillEvaluator", menuName = "Kafelki/AI/SkillEvaluators/RandomSkillEvaluator", order = 0)]
	public class RandomSkillEvaluator : SkillEvaluator
	{
		public float Min;
		public float Max;

		public override float EvaluateSkill(ISkillEvaluationContext context, Skill skill, GameEntity entity)
		{
			float span = Max - Min;
			return Min + context.Rng.NextFloat() * span;
		}
	}
}