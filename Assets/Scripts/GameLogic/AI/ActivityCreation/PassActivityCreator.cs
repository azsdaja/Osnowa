namespace GameLogic.AI.ActivityCreation
{
	using ActionLoop.Activities;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;

	[CreateAssetMenu(fileName = "PassActivityCreator", menuName = "Kafelki/AI/Activities/PassActivityCreator", order = 0)]
	public class PassActivityCreator : ActivityCreator
	{
		protected override Activity CreateActivityInternal(IActivityCreationContext context, StimulusContext stimulusContext, GameEntity entity)
		{
			if (entity.hasView && entity.view.Controller is EntityViewBehaviour)
			{
				var evb = (EntityViewBehaviour) entity.view.Controller;
				evb.SortingOrder = 0;
			}

			return new WaitActivity(context.ActionFactory, 1, "Pass");
		}
	}
}