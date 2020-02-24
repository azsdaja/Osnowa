namespace GameLogic.AI
{
	using Model;

	public interface IStimulusReceiver
	{
		void HandleStimulus(GameEntity stimulusTarget, StimulusType stimulusType, GameEntity stimulusSource);
		void Unnotice(GameEntity entity, GameEntity unnoticedActor);
		void Notice(GameEntity noticedEntity, StimulusType stimulusType, GameEntity entity, bool noticingEnemy);
	}
}