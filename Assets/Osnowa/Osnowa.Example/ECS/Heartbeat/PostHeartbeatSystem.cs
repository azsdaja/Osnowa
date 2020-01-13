using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Osnowa.Osnowa.Context;
using Osnowa.Osnowa.Core.CSharpUtilities;
using Osnowa.Osnowa.Grid;
using UnityEngine;

namespace Osnowa.Osnowa.Example.ECS.Heartbeat
{
	using GameLogic;
	using GameLogic.ActionLoop;
	using GameLogic.AI;
	using GameLogic.Entities;
	using GameLogic.GridRelated;
	using Pathfinding;
	using UI;

	public class PostHeartbeatSystem : ReactiveSystem<GameEntity>
	{
		private readonly IEntityDetector _entityDetector;
		private readonly IGameConfig _gameConfig;
		private readonly ICalculatedAreaAccessor _calculatedAreaAccessor;
		private readonly IStimulusReceiver _stimulusReceiver;
		private readonly IPathfinder _pathfinder;
		private readonly IBroadcastStimulusSender _broadcastStimulusSender;
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IFriendshipResolver _friendshipResolver;
		private GameContext _context;
		private readonly IUiFacade _uiFacade;
		private readonly IOsnowaContextManager _contextManager;

		public PostHeartbeatSystem(IEntityDetector entityDetector, IGameConfig gameConfig,
			ICalculatedAreaAccessor calculatedAreaAccessor, IStimulusReceiver stimulusReceiver, IPathfinder pathfinder,
			IBroadcastStimulusSender broadcastStimulusSender, IPositionEffectPresenter positionEffectPresenter,
            GameContext context, IFriendshipResolver friendshipResolver,
			IUiFacade uiFacade, IOsnowaContextManager contextManager)
			: base(context)
		{
			_entityDetector = entityDetector;
			_gameConfig = gameConfig;
			_calculatedAreaAccessor = calculatedAreaAccessor;
			_stimulusReceiver = stimulusReceiver;
			_pathfinder = pathfinder;
			_broadcastStimulusSender = broadcastStimulusSender;
			_positionEffectPresenter = positionEffectPresenter;
			_friendshipResolver = friendshipResolver;
			_uiFacade = uiFacade;
			_contextManager = contextManager;
			_context = context;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.FinishedTurn);
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isFinishedTurn;
		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (GameEntity entity in entities)
			{
				try
				{
					PostHeartbeatEntity(entity);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
				}
			}
		}

		private void PostHeartbeatEntity(GameEntity entity)
		{
			if (!entity.hasVision)
			{
				return;
			}
		    List<GameEntity> entitiesInVicinity = _entityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRay).ToList();
            if (!entity.isPlayerControlled)
			{
				HashSet<GameEntity> seenActorsToVerifyIfStillInRange = entity.vision.EntitiesRecentlySeen.ToHashSet();
				/* służy do rozstrzygnięcia, czy wysyłać NoticeEnemy
				 
				 foreach (GameEntity entityInVicinity in entitiesInVicinity)
				{
					ProcessEntityInVicinity(entity, entityInVicinity, entity.hasAware, seenActorsToVerifyIfStillInRange);
				}*/
				foreach (GameEntity actorThatGotOutOfRange in seenActorsToVerifyIfStillInRange)
				{
					_stimulusReceiver.Unnotice(entity, actorThatGotOutOfRange);
				}
			}
		}

/*
		private void ProcessEntityInVicinity(GameEntity entity, GameEntity entityInVicinity, bool noticingEnemies, HashSet<GameEntity> seenActorsToVerify)
		{
			if (entity.vision.EntitiesNoticed.Contains(entityInVicinity.id.Id))
			{
				seenActorsToVerify.Remove(entityInVicinity);
				return;
			}

			bool isEnemy = !_friendshipResolver.AreFriends(entity, entityInVicinity);

			if (isEnemy)
			{
				FovArea enemyFov = _calculatedAreaAccessor.FetchVisibilityFov(entityInVicinity.position.Position, entity.vision.VisionRay);

				// cheat (it's really checking if current actor is in sight of enemy). todo: use expression area of enemy actor (below) instead?
				bool enemyIsInSight = enemyFov.Positions.Contains(entity.position.Position);
				if (enemyIsInSight)
				{
					if (noticingEnemies /* || entity.suspiciousness.Suspiciousness >= _gameConfig.SuspiciousnessForAware#1#)
						_stimulusSender.OnSendStimulus(entity, _gameConfig.StimuliDefinitions.NoticeEnemy, entityInVicinity);
				}
			}
		}
*/
	}
}