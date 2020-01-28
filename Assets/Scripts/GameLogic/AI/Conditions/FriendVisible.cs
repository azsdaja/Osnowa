namespace GameLogic.AI.Conditions
{
	using System.Linq;
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "FriendVisible", menuName = "Osnowa/AI/Conditions/FriendVisible", order = 0)]
	public class FriendVisible : Condition
	{
		public override bool Evaluate(GameEntity entity, IConditionContext conditionContext)
		{
			return entity.vision.EntitiesNoticed
				.Select(id => conditionContext.Context.GetEntityWithId(id))
				.Any(e => conditionContext.FriendshipResolver.AreFriends(entity, e));
		}
	}
}