namespace GameLogic.AI
{
	using Model;
	using Osnowa.Osnowa.AI.Activities;

	public interface IActivityResolver
	{
		IActivity ResolveNewActivityForActorIfApplicable(StimulusType stimulusType, GameEntity targetActor, GameEntity entity);
	}
}