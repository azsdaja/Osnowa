using System.Collections.Generic;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS
{
	using GameLogic.Entities;

	public class LoadViewSystem : ReactiveSystem<GameEntity>
	{
		private readonly IViewCreator _viewCreator;

		public LoadViewSystem(IViewCreator viewCreator, IContext<GameEntity> context) : base(context)
		{
			_viewCreator = viewCreator;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.Position);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasPosition && !entity.hasView;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var e in entities)
			{
				IViewController viewController = _viewCreator.InitializeViewForEntity(e);
				if (viewController != null) e.ReplaceView(viewController);
			}
		}
	}
}