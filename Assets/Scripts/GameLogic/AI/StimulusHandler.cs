namespace GameLogic.AI
{
	using System;
	using System.Collections.Generic;
	using ActionLoop;
	using Entities;
	using Model;
	using Osnowa.Osnowa.AI.Activities;

	public class StimulusHandler : IStimulusHandler
	{
		private readonly IGameConfig _gameConfig;
		private readonly IActivityResolver _activityResolver;
		private readonly ISceneContext _sceneContext;
		private readonly IFriendshipResolver _friendshipResolver;
		private readonly IActivityInterruptor _activityInterruptor;

		public StimulusHandler(IGameConfig gameConfig, IActivityResolver activityResolver, ISceneContext sceneContext,
			IFriendshipResolver friendshipResolver, IActivityInterruptor activityInterruptor)
		{
			_gameConfig = gameConfig;
			_activityResolver = activityResolver;
			_sceneContext = sceneContext;
			_friendshipResolver = friendshipResolver;
			_activityInterruptor = activityInterruptor;
		}

		public void Unnotice(GameEntity entity, Stimulus stimulus)
		{
			GameEntity unnoticedEntity = Contexts.sharedInstance.game.GetEntityWithId(stimulus.ObjectEntityId);
			if (!_friendshipResolver.AreFriends(entity, unnoticedEntity))
			{
				if (entity.hasAware)
				{
					entity.aware.TurnsLeft = 8;
				}
			}
			
			/*	todo: how to handle situation like below?
            				1. an enemy is noticed
            				2. he dies and is removed from the context
            				3. next time current actor checks the entities in FOV, the enemy is not detected, so we unnotice him
            				4. _friendshipResolver doesn't know if unnoticed entity was enemy or not*/

		}

		public void Notice(GameEntity entity, Stimulus stimulus, bool noticingEnemy)
		{
			HashSet<Guid> entitiesSeen = entity.vision.EntitiesNoticed;
			entitiesSeen.Add(entity.id.Id);
			entity.ReplaceVision(entity.vision.VisionRange, entity.vision.PerceptionRange, entitiesSeen);

			if(noticingEnemy && entity.hasAware)
				entity.ReplaceAware(int.MaxValue);
		}

		public void Unnotice(GameEntity unnoticedEntity, GameEntity entity)
		{
		}
	}
}