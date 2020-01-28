namespace GameLogic.AI.ActivityCreation
{
	using System.Linq;
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using UnityEngine;

	[CreateAssetMenu(fileName = "AttackActivityCreator", menuName = "Osnowa/AI/Activities/AttackActivityCreator", order = 0)]
	public class AttackActivityCreator : ActivityCreator
	{
		public int GiveUpDistance = 8;

		protected override Activity CreateActivityInternal(IActivityCreationContext context, 
			StimulusContext stimulusContext, GameEntity entity)
		{
			GameEntity enemyNearby = context.EntityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRay)
				.FirstOrDefault(e => !context.FriendshipResolver.AreFriends(entity, e));
			return new AttackActivity(context.ActionFactory, context.Navigator, GiveUpDistance, enemyNearby, "Attack", context.Rng, context.GameConfig, context.EntityDetector);
		}
	}
}