using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Player
{
	using GameLogic;

	public class PresentPlayerSatiationSystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext _context;
		private readonly IUiFacade _uiFacade;

		public PresentPlayerSatiationSystem(GameContext context, IUiFacade uiFacade) : base(context)
		{
			_context = context;
			_uiFacade = uiFacade;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.PlayerControlled, GameMatcher.Stomach));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isPlayerControlled && entity.hasStomach;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			GameEntity playerEntity = entities.SingleEntity();
			_uiFacade.SetSatiation(playerEntity.stomach.Satiation, playerEntity.stomach.MaxSatiation);
		}
	}
}
