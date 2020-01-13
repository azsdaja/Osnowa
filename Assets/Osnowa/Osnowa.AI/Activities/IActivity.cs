namespace Osnowa.Osnowa.AI.Activities
{
	public interface IActivity
	{
		ActivityStep CheckAndResolveStep(GameEntity entity);
		string GetFullName();
		string Name { get; }
		float Score { get; set; }

		void OnSuccess(GameEntity entity);
		void OnFailure(GameEntity entity);
	}
}