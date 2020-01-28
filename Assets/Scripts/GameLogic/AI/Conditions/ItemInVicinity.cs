namespace GameLogic.AI.Conditions
{
	using System.Collections.Generic;
	using System.Linq;
	using Model;
	using PCG.Recipees;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ItemInVicinity", menuName = "Osnowa/AI/Conditions/ItemInVicinity", order = 0)]
	public class ItemInVicinity : Condition
	{
		public EntityRecipee ItemRecipee;

		public override bool Evaluate(GameEntity entity, IConditionContext conditionContext)
		{
			IEnumerable<GameEntity> selectedItemsInVicinity = conditionContext.EntityDetector.DetectEntities(entity.position.Position,
					entity.vision.PerceptionRange)
					.Where(e => e.isCarryable)
					.Where(e => e.recipee.RecipeeName == ItemRecipee.Id);
			return selectedItemsInVicinity.Any();
		}
	}
}