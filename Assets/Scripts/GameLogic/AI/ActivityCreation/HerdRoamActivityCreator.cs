namespace GameLogic.AI.ActivityCreation
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using ActionLoop.Activities;
	using Entities;
	using Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using UnityEngine;

	/// <summary>
	/// Creates an activity that is similar to Roam, but makes the actor keep close to friendly entities.
	/// </summary>
	[CreateAssetMenu(fileName = "HerdRoamActivityCreator", menuName = "Osnowa/AI/Activities/HerdRoamActivityCreator", order = 0)]
	public class HerdRoamActivityCreator : ActivityCreator
	{
		[Range(5, 30)]
		public int MaxWalkawayRangeFromHerdCenter = 10;

		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, 
			GameEntity entity)
		{
			Position herdCenter = GetHerdCenterOf(context.EntityDetector, entity, context.FriendshipResolver);
			var betweenMeAndHerdCenter = new Position((herdCenter.x + entity.position.Position.x) / 2, (herdCenter.y + entity.position.Position.y) / 2);
			NavigationData navigationData;
			try
			{
				navigationData = GetReachableNavigationDataForHerdWander(context, entity, betweenMeAndHerdCenter, MaxWalkawayRangeFromHerdCenter);
			}
			catch (Exception)
			{
				return new WaitActivity(context.ActionFactory, 10, "Wait");
			}
			return new SequenceActivity(new List<IActivity>
			{
				new GoToActivity(context.ActionFactory, navigationData, context.Navigator, "Roam"),
				new WaitActivity(context.ActionFactory, context.Rng.Next(15), "Wait")
			}.GetEnumerator(), "Herd roam");
		}

		private Position GetHerdCenterOf(IEntityDetector detector, GameEntity entity, IFriendshipResolver friendshipResolver)
		{
			List<GameEntity> meAndFriendsAround = detector.DetectEntities(entity.position.Position, entity.vision.PerceptionRange)
				.Where(e => friendshipResolver.AreFriends(entity, e))
				.Union(new[]{ entity })
				.ToList();
			int averageX = meAndFriendsAround.Sum(f => f.position.Position.x)/meAndFriendsAround.Count;
			int averageY = meAndFriendsAround.Sum(f => f.position.Position.y)/meAndFriendsAround.Count;

			//Debug.Log($"me and friends: {meAndFriendsAround.Count}");

			return new Position(averageX, averageY);
		}

		private NavigationData GetReachableNavigationDataForHerdWander(IActivityCreationContext context, 
			GameEntity entity, Position targetAreaCenter, int wanderRange)
		{
			var stopwatch = Stopwatch.StartNew();
			
			NavigationData newNavigationData;
			const int maxAttemptsBeforeGivingUpWithStraightPath = 10;
			int attempts = 0;
			while (true)
			{
				++attempts;
				Position wanderVector = new Position(context.Rng.Next(-wanderRange, wanderRange), context.Rng.Next(-wanderRange, wanderRange));
				Position newTargetPosition = targetAreaCenter + wanderVector;
				if (newTargetPosition == entity.position.Position)
					continue;

				List<Position> pathToTarget = attempts <= maxAttemptsBeforeGivingUpWithStraightPath
					? context.Navigator.GetStraightPath(entity.position.Position, newTargetPosition)
					: context.Navigator.GetJumpPoints(entity.position.Position, newTargetPosition);

				if (pathToTarget == null)
				{
					continue;
				}

				newNavigationData = context.Navigator.GetNavigationData(entity.position.Position, newTargetPosition);

				break;
			}
			//Debug.Log("FOUND GOOD PLACE AFTER " + attempts + " attempts and " + stopwatch.ElapsedMilliseconds + "ms.");
			return newNavigationData;
		}

	}
}