namespace GameLogic.ActionLoop
{
	using AI.Model;

	public interface IBroadcastStimulusSender
	{
		void OnSendStimulus(GameEntity activeEntity, int range, StimulusDefinition stimulusDefinition);
	}
}