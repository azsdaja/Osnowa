namespace GameLogic.AI.SkillEvaluation
{
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "TimeOfDayDependentSkillEvaluator", menuName = "Osnowa/AI/SkillEvaluators/TimeOfDayDependentSkillEvaluator", order = 0)]
	public class TimeOfDayDependentSkillEvaluator : SkillEvaluator
	{
		public AnimationCurve TimeToScore;

		public override float EvaluateSkill(ISkillEvaluationContext context, Skill skill, GameEntity entity)
		{
			float normalizedTimeOfDay = context.ContextManager.Current.InGameDate.Hour/24f;
			return TimeToScore.Evaluate(normalizedTimeOfDay);
		}
	}
}