namespace GameLogic.ActionLoop
{
	using System.Collections.Generic;
	using System.Linq;
	using AI.Model;
	using Osnowa.Osnowa.Grid;

	class BroadcastStimulusSender : IBroadcastStimulusSender
	{
		private readonly IEntityDetector _entityDetector;

		public BroadcastStimulusSender(IEntityDetector entityDetector)
		{
			_entityDetector = entityDetector;
		}

		public void OnSendStimulus(GameEntity activeEntity, int range, StimulusDefinition stimulusDefinition)
		{
			IEnumerable<GameEntity> otherActorsAround = _entityDetector.DetectEntities(activeEntity.position.Position, range)
				.Where(a => a != activeEntity);

			foreach (GameEntity receiver in otherActorsAround)
			{
//				_stimulusSender.OnSendStimulus(receiver, stimulusDefinition, activeEntity);
			}
		}
	}
}