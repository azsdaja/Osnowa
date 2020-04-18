namespace GameLogic.AI
{
	using Model;

	public interface IStimulusHandler
	{
		void Unnotice(GameEntity entity, Stimulus stimulus);
		void Notice(GameEntity entity, Stimulus stimulus, bool noticingEnemy);
	}
}