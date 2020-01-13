namespace GameLogic.ActionLoop.Activities
{
	using AI;
	using AI.Navigation;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.RNG;
	using UI;

	// todo: just a stub made from TalkActivity
	public class DanceActivity : Activity
	{
		private readonly IActionFactory _actionFactory;
		private readonly INavigator _navigator;
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IRandomNumberGenerator _rng;
		private readonly ISceneContext _sceneContext;

		private GameEntity _targetActor;
		private NavigationData _navigationData;

		private int _turnsCountOfLastResponse;
		private ConversationState _conversationState;

		public DanceActivity(IActionFactory actionFactory, INavigator navigator, StimulusContext stimulusContext, 
			string name, IPositionEffectPresenter positionEffectPresenter, IRandomNumberGenerator rng, ISceneContext sceneContext) : base(name)
		{
			_actionFactory = actionFactory;
			_navigator = navigator;
			_targetActor = stimulusContext?.TargetEntity;
			_positionEffectPresenter = positionEffectPresenter;
			_rng = rng;
			_sceneContext = sceneContext;

			bool talkInitiatedBySomeoneElse = stimulusContext != null;
			_conversationState = talkInitiatedBySomeoneElse ? ConversationState.WaitingForAnswer : ConversationState.WalkingTowardsTarget;
			_navigationData = null;
		}

		// 1. odzywa się, po czym wpada w stan, w którym podchodzi do rozmówcy i nawt jak dojdzie to czeka np. 10 tur. 
		// Jeśli otrzyma słowo, to idzie do rozmówcy i stoi też, ale w losowym momencie po chwili się odzywa.
		public override ActivityStep ResolveStep(GameEntity entity)
		{
			return Fail(entity);
		}
	}
}