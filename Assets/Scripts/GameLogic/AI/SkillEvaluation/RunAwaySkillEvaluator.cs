namespace GameLogic.AI.SkillEvaluation
{
	using System.Collections.Generic;
	using System.Linq;
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "RunAwaySkillEvaluator", menuName = "Osnowa/AI/SkillEvaluators/RunAwaySkillEvaluator", order = 0)]
	public class RunAwaySkillEvaluator : SkillEvaluator
	{
		public override float EvaluateSkill(ISkillEvaluationContext context, Skill skill, GameEntity entity)
		{
			IEnumerable<GameEntity> actorsAround = 
				context.EntityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRange)
				.Where(a => a!= entity);
			if (!actorsAround.Any())
				return 0f;

			GameEntity closestActor = actorsAround.First(); // todo: needs finishing

			return 0f;
		}
	}
}