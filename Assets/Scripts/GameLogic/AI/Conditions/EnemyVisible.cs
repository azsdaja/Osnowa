namespace GameLogic.AI.Conditions
{
	using System.Collections.Generic;
	using System.Linq;
	using Model;
	using UnityEngine;

	[CreateAssetMenu(fileName = "EnemyVisible", menuName = "Kafelki/AI/Conditions/EnemyVisible", order = 0)]
	public class EnemyVisible : Condition
	{
		public override bool Evaluate(GameEntity entity, IConditionContext conditionContext)
		{
			IEnumerable<GameEntity> entitiesSeen = entity.vision.EntitiesNoticed
				.Select(guid => conditionContext.Context.GetEntityWithId(guid))
				.Where(e => e != null) // SAME AS IN PostHeartbeatSystem. will ignore removed entites, but they will remain in EntitiesNoticed. todo: is it fine?
				;
			
			return entitiesSeen.Any(e => !conditionContext.FriendshipResolver.AreFriends(entity, e));
		}
	}
}