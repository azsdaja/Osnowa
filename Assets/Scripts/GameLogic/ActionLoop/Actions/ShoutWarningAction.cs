namespace GameLogic.ActionLoop.Actions
{
	using System.Collections.Generic;
	using ActionEffects;
	using Osnowa.Osnowa.Core.ActionLoop;
	using UI;

	public class ShoutWarningAction : GameAction
	{
		private readonly IBroadcastStimulusSender _broadcastStimulusSender;
		private readonly IGameConfig _gameConfig;
		private readonly IPositionEffectPresenter _positionEffectPresenter;

		public ShoutWarningAction(GameEntity entity, float energyCost, IBroadcastStimulusSender broadcastStimulusSender, 
			IGameConfig gameConfig, IPositionEffectPresenter positionEffectPresenter, IActionEffectFactory actionEffectFactory) 
			: base(entity, energyCost, actionEffectFactory)
		{
			_broadcastStimulusSender = broadcastStimulusSender;
			_gameConfig = gameConfig;
			_positionEffectPresenter = positionEffectPresenter;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
//			_broadcastStimulusSender.OnSendStimulus(Entity, 30, _gameConfig.StimuliDefinitions.ShoutWarning);

			return new[]{new LambdaEffect(actorBehaviour =>
			{ }, Entity.view.Controller)};
		}
	}
}