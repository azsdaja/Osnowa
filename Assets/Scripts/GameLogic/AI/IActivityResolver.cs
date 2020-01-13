namespace GameLogic.AI
{
	using Model;
	using Osnowa.Osnowa.AI.Activities;

	public interface IActivityResolver
	{
		IActivity ResolveNewActivityForActorIfApplicable(StimulusDefinition stimulus, GameEntity targetActor, GameEntity entity);
	}
}