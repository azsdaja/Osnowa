namespace Osnowa.Osnowa.Unity
{
	using Core;
	using GameLogic.Entities;
	using UnityUtilities;

	public class ViewCreator : IViewCreator
	{
		private readonly IEntityViewBehaviourInitializer _entityViewBehaviourInitializer;
		private readonly IEntityGenerator _entityGenerator;
		private readonly GameContext _context;

		public ViewCreator(IEntityViewBehaviourInitializer entityViewBehaviourInitializer, 
			IEntityGenerator entityGenerator, GameContext context)
		{
			_entityViewBehaviourInitializer = entityViewBehaviourInitializer;
			_entityGenerator = entityGenerator;
			_context = context;
		}

		public IViewController SpawnEntity(IEntityRecipee entityRecipee, Position position, bool forceAggressive = true)
		{
			GameEntity entity;
			_entityGenerator.GenerateActorFromRecipeeAndAddToContext(_context, entityRecipee, position, out entity);
			return InitializeViewForEntity(entity);
		}

		public IViewController InitializeViewForEntity(GameEntity entity)
		{
			EntityViewBehaviour instantiatedEntityView = PoolingManager.Fetch(PoolingManager.EntityView).GetComponent<EntityViewBehaviour>();
			instantiatedEntityView.Entity = entity;
			entity.ReplaceView(instantiatedEntityView);

			_entityViewBehaviourInitializer.Initialize(instantiatedEntityView);
			
			return instantiatedEntityView;
		}
	}
}