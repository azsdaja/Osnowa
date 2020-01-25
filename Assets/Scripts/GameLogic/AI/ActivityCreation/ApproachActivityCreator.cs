namespace GameLogic.AI.ActivityCreation
{
	using System;
	using System.Linq;
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ApproachActivityCreator", menuName = "Osnowa/AI/Activities/ApproachActivityCreator", order = 0)]
	public class ApproachActivityCreator : ActivityCreator
	{
		protected override Activity CreateActivityInternal(IActivityCreationContext context, 
			StimulusContext stimulusContext, GameEntity entity)
		{
			GameEntity enemyNearby = context.EntityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRay)
				.FirstOrDefault(e => !context.FriendshipResolver.AreFriends(entity, e));
			Func<Position> targetPositionGetter = () => enemyNearby.position.Position;
			return new ApproachActivity(context.ActionFactory, context.Navigator, targetPositionGetter, 10, "Approach");
		}
	}
}