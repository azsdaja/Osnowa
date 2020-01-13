namespace GameLogic.AI
{
	using Model;

	public interface IStimulusReceiver
	{
		void HandleStimulus(GameEntity stimulusTarget, StimulusDefinition stimulus, GameEntity stimulusSource);
		void Unnotice(GameEntity entity, GameEntity unnoticedActor);
		void Notice(GameEntity noticedEntity, StimulusDefinition stimulus, GameEntity entity, bool noticingEnemy);
	}
}