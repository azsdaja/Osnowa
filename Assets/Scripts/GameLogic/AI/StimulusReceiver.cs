namespace GameLogic.AI
{
	using System;
	using System.Collections.Generic;
	using ActionLoop;
	using Entities;
	using Model;
	using Osnowa.Osnowa.AI.Activities;

	public class StimulusReceiver : IStimulusReceiver
	{
		private readonly IGameConfig _gameConfig;
		private readonly IActivityResolver _activityResolver;
		private readonly ISceneContext _sceneContext;
		private readonly IFriendshipResolver _friendshipResolver;
		private readonly IActivityInterruptor _activityInterruptor;

		public StimulusReceiver(IGameConfig gameConfig, IActivityResolver activityResolver, ISceneContext sceneContext,
			IFriendshipResolver friendshipResolver, IActivityInterruptor activityInterruptor)
		{
			_gameConfig = gameConfig;
			_activityResolver = activityResolver;
			_sceneContext = sceneContext;
			_friendshipResolver = friendshipResolver;
			_activityInterruptor = activityInterruptor;
		}

		public void HandleStimulus(GameEntity stimulusTarget, StimulusType stimulusType, GameEntity stimulusSource)
		{
			/*if (stimulus == _gameConfig.StimuliDefinitions.NoticeFriend || stimulus == _gameConfig.StimuliDefinitions.NoticeEnemy)
			{
				Notice(stimulusSource, stimulus, stimulusTarget, stimulus == _gameConfig.StimuliDefinitions.NoticeEnemy);
			}*/
		}

		public void Notice(GameEntity noticedEntity, StimulusType stimulusType, GameEntity entity, bool noticingEnemy)
		{
			HashSet<Guid> entitiesSeen = entity.vision.EntitiesNoticed;
			entitiesSeen.Add(noticedEntity.id.Id);
			entity.ReplaceVision(entity.vision.VisionRange, entity.vision.PerceptionRange, entitiesSeen);

			if(noticingEnemy && entity.hasAware)
				entity.ReplaceAware(int.MaxValue);
			
			IActivity activityFromReaction = _activityResolver.ResolveNewActivityForActorIfApplicable(stimulusType, noticedEntity, entity);
			if (activityFromReaction == null)
				return;

			if(entity.hasActivity)
				_activityInterruptor.FailAndReplace(entity, entity.activity.Activity, activityFromReaction);
			else entity.AddActivity(activityFromReaction);
		}

		public void Unnotice(GameEntity entity, GameEntity unnoticedActor)
		{
			entity.vision.EntitiesNoticed.Remove(unnoticedActor.id.Id);
			if (!_friendshipResolver.AreFriends(entity, unnoticedActor))
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
	}
}