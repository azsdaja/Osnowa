namespace GameLogic.AI.ActivityCreation
{
	using System.Collections.Generic;
	using ActionLoop.Activities;
	using Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	[CreateAssetMenu(fileName = "RoamActivityCreator", menuName = "Kafelki/AI/Activities/RoamActivityCreator", order = 0)]
	public class RoamActivityCreator : ActivityCreator
	{
		public int RoamRange = 5;
		public int MaxWaitTime = 14;

		private int[] _hits = new int[5];
		private int _misses;

		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, GameEntity entity)
		{
			NavigationData navigationData1 = GetReachableNavigationDataForWander(context, entity.position.Position, entity.position.Position, RoamRange, entity);
			NavigationData navigationData2 = GetReachableNavigationDataForWander(context, navigationData1.Destination, entity.position.Position, RoamRange, entity);
			return new SequenceActivity(new List<IActivity>
			{
				new WaitActivity(context.ActionFactory, context.Rng.Next(MaxWaitTime) + 1, "Wait 1/2"),
				new GoToActivity(context.ActionFactory, navigationData1, context.Navigator, "GoTo 1/2"),
				new WaitActivity(context.ActionFactory, context.Rng.Next(MaxWaitTime) + 1, "Wait 1/2"),
				new GoToActivity(context.ActionFactory, navigationData2, context.Navigator, "GoTo 2/2"),
				new WaitActivity(context.ActionFactory, context.Rng.Next(MaxWaitTime) + 1, "Wait 2/2"),
			}.GetEnumerator(), "Roam");
		}

		private NavigationData GetReachableNavigationDataForWander(IActivityCreationContext context, Position startPosition, Position targetAreaCenter, 
			int wanderRange, GameEntity entity)
		{
			NavigationData newNavigationData;
			while (true)
		    {
		        Position newTargetPosition;
		        Position wanderVector = new Position(context.Rng.Next(-wanderRange, wanderRange + 1),
		            context.Rng.Next(-wanderRange, wanderRange + 1));
		        newTargetPosition = targetAreaCenter + wanderVector;

		        if (newTargetPosition == startPosition)
		            continue;

		        newNavigationData = context.Navigator.GetNavigationData(startPosition, newTargetPosition);

		        if (newNavigationData == null)
		        {
		            continue;
		        }
		        break;
		    }
		    return newNavigationData;
		}

	}
}