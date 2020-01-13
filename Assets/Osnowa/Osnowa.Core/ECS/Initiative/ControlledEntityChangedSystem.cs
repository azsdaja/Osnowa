using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Initiative
{
	/// <summary>
    /// Performs any updates necessary after changing controlled entity.
    /// </summary>
	public class ControlledEntityChangedSystem : ReactiveSystem<GameEntity>
	{
		private readonly GameContext _context;

		public ControlledEntityChangedSystem(GameContext context) : base(context)
		{
			_context = context;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.PlayerControlled, GameMatcher.View));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isPlayerControlled && entity.hasView;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			GameEntity singleEntity = entities.SingleEntity();

			singleEntity.view.Controller.SetAsActiveActor();
			_context.ReplacePlayerEntity(singleEntity.id.Id);
		}
	}
}