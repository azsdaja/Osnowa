namespace GameLogic.AI.ActivityCreation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	[CreateAssetMenu(fileName = "RunAwayActivityCreator", menuName = "Kafelki/AI/Activities/RunAwayActivityCreator", order = 0)]
	public class RunAwayActivityCreator : ActivityCreator
	{
		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, GameEntity entity)
		{
			IEnumerable<GameEntity> entitiesSeen = entity.vision.EntitiesRecentlySeen;
			GameEntity enemyNearby = entitiesSeen.FirstOrDefault(e => !context.FriendshipResolver.AreFriends(entity, e));
			if (enemyNearby == null)
			{
				throw new InvalidOperationException("Running away, but from whom?");
			}
			return new RunAwayActivity(context.ActionFactory, context.CalculatedAreaAccessor, enemyNearby.position.Position, "Run away");
		}
	}
}