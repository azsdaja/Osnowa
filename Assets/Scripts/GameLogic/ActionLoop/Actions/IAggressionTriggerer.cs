namespace GameLogic.ActionLoop.Actions
{
	public interface IAggressionTriggerer
	{
		void TriggerAggressionIfEligible(GameEntity target);
	}
}