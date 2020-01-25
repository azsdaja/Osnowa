namespace GameLogic.AI.ActivityCreation
{
	using System.Linq;
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	[CreateAssetMenu(fileName = "KeepDistanceActivityCreator", menuName = "Osnowa/AI/Activities/KeepDistanceActivityCreator", order = 0)]
	public class KeepDistanceActivityCreator : ActivityCreator
	{
		public int PreferredDistance;

		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, 
			GameEntity entity)
		{
			GameEntity enemyNearby =
				context.EntityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRay)
					.FirstOrDefault(e => !context.FriendshipResolver.AreFriends(entity, e));
			return new KeepDistanceActivity(context.ActionFactory, PreferredDistance, context.Navigator, enemyNearby, 
				context.CalculatedAreaAccessor, context.Rng, "Keep distance", context.ContextManager);
		}
	}
}