using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Body
{
	using GameLogic;

	public class IntegrityChangedSystem : ReactiveSystem<GameEntity>
	{
		private readonly IDeathHandler _deathHandler;
		private readonly IUiFacade _uiFacade;

		public IntegrityChangedSystem(IContext<GameEntity> context, IDeathHandler deathHandler, IUiFacade uiFacade) : base(context)
		{
			_deathHandler = deathHandler;
			_uiFacade = uiFacade;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.View, GameMatcher.Integrity));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView && entity.hasIntegrity;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				if (entity.integrity.Integrity <= 0)
				{
					_deathHandler.HandleDeath(entity);
				}

				float integrityRatio = entity.integrity.Integrity/entity.integrity.MaxIntegrity;
				entity.view.Controller.UiPresenter.SetIntegrityRatio(integrityRatio);

				if (entity.isPlayerControlled)
				{
					_uiFacade.SetHealth((int) entity.integrity.Integrity, (int) entity.integrity.MaxIntegrity);
				}
			}
		}
	}
}