using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Statuses
{
	using GameLogic.Entities;
	using NaPozniej;

	public class AnyStatusChangedSystem : ReactiveSystem<GameEntity>
	{
		private readonly IGameConfig _gameConfig;

		public AnyStatusChangedSystem(IContext<GameEntity> context, IGameConfig gameConfig) : base(context)
		{
			_gameConfig = gameConfig;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(
				GameMatcher.AnyOf(GameMatcher.Aggressive, GameMatcher.Sleeping)
				.AddedOrRemoved()
				);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				ActorStatusDefinition suspiciousnessRelatedStatus = null;
				if (entity.isAggressive)
					suspiciousnessRelatedStatus = _gameConfig.ActorStatuses.Aware;
				entity.view.Controller.SetStatus(ViewStatusClass.SuspiciousnessRelated, suspiciousnessRelatedStatus);
			}
		}
	}
}