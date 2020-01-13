namespace Osnowa.Osnowa.Unity
{
	using Example;
	using GameLogic.Entities;
	using global::Osnowa.Osnowa.Context;
	using UnityEngine;

	public class EntityViewBehaviourInitializer : IEntityViewBehaviourInitializer
	{
		private readonly ISceneContext _sceneContext;
		private readonly GameContext _context;
		private readonly IGameConfig _gameConfig;
		private readonly IOsnowaContextManager _contextManager;

		public EntityViewBehaviourInitializer(ISceneContext sceneContext, GameContext context, IGameConfig gameConfig, IOsnowaContextManager contextManager)
		{
			_sceneContext = sceneContext;
			_context = context;
			_gameConfig = gameConfig;
			_contextManager = contextManager;
		}

		public void Initialize(EntityViewBehaviour entityViewBehaviour)
		{
			GameEntity entity = entityViewBehaviour.Entity;

			entityViewBehaviour.SetStatus(ViewStatusClass.SuspiciousnessRelated, null);
			entityViewBehaviour.transform.parent = _sceneContext.EntitiesParent;
			if (entity.hasPosition)
			{
				entityViewBehaviour.SetSprite(entity.looks.BodySprite, Color.white);
				entityViewBehaviour.RefreshWorldPosition();
			}

            entityViewBehaviour.name = entity.recipee.RecipeeName;

            if (_gameConfig.ModeConfig.Vision != Vision.Permanent && !_contextManager.Current.VisibleEntities.Contains(entity.view.Controller))
			{
				entityViewBehaviour.Hide();
			}
			entityViewBehaviour.gameObject.SetActive(true);
			entityViewBehaviour.EntityUiPresenter.SetIntegrityRatio(entity.hasIntegrity ? entity.integrity.Integrity / entity.integrity.MaxIntegrity : 1f);
		}
	}
} 