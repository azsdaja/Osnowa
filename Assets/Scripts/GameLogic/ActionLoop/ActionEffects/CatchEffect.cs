namespace GameLogic.ActionLoop.ActionEffects
{
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Unity;

	public class CatchEffect : IActionEffect
	{
		private readonly EntityViewBehaviour _caughActorBehaviour;
		private readonly EntityViewBehaviour _actorDataEntity;

		public CatchEffect(EntityViewBehaviour caughActorBehaviour, EntityViewBehaviour actorDataEntity)
		{
			_caughActorBehaviour = caughActorBehaviour;
			_actorDataEntity = actorDataEntity;
		}

		public void Process()
		{
		}
	}
}