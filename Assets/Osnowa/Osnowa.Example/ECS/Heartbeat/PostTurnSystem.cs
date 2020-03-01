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
	using GameLogic.AI.Model;
	using GameLogic.Entities;
	using GameLogic.GridRelated;
	using Pathfinding;
	using UI;

	public class PostTurnSystem : ReactiveSystem<GameEntity>
	{
		private readonly IEntityDetector _entityDetector;
		private readonly IGameConfig _gameConfig;
		private readonly ICalculatedAreaAccessor _calculatedAreaAccessor;
		private readonly IStimulusHandler _stimulusHandler;
		private readonly IPathfinder _pathfinder;
		private readonly IBroadcastStimulusSender _broadcastStimulusSender;
		private readonly IPositionEffectPresenter _positionEffectPresenter;
		private readonly IFriendshipResolver _friendshipResolver;
		private GameContext _context;
		private readonly IUiFacade _uiFacade;
		private readonly IOsnowaContextManager _contextManager;

		public PostTurnSystem(IEntityDetector entityDetector, IGameConfig gameConfig,
			ICalculatedAreaAccessor calculatedAreaAccessor, IStimulusHandler stimulusHandler, IPathfinder pathfinder,
			IBroadcastStimulusSender broadcastStimulusSender, IPositionEffectPresenter positionEffectPresenter,
            GameContext context, IFriendshipResolver friendshipResolver,
			IUiFacade uiFacade, IOsnowaContextManager contextManager)
			: base(context)
		{
			_entityDetector = entityDetector;
			_gameConfig = gameConfig;
			_calculatedAreaAccessor = calculatedAreaAccessor;
			_stimulusHandler = stimulusHandler;
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
					PostTurnEntity(entity);
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
				}
			}
		}

		private void PostTurnEntity(GameEntity entity)
		{
			if (entity.hasStimuli)
			{
				entity.RemoveStimuli();
			}
			if (!entity.hasVision)
			{
				return;
			}
		    List<GameEntity> entitiesInVicinity = _entityDetector.DetectEntities(entity.position.Position, entity.vision.VisionRange)
			    .Where(e => e != entity)
			    .ToList();
		    if (entity.isPlayerControlled)
		    {
			    return;
		    }

            HashSet<GameEntity> entitiesToUnnotice = new HashSet<GameEntity>();
            foreach (GameEntity entityInVicinity in entitiesInVicinity)
            {
	            ProcessEntityInVicinity(entity, entityInVicinity, entitiesToUnnotice);
	            
            }
            foreach (GameEntity entityToUnnotice in entitiesToUnnotice)
            {
	            entity.vision.EntitiesNoticed.Remove(entityToUnnotice.id.Id);
	            _stimulusHandler.Unnotice(entity, new Stimulus
	            {
		            Type = StimulusType.IUnnotice,
		            ObjectEntityId = entityToUnnotice.id.Id 
	            });
            }
		}

		private void ProcessEntityInVicinity(GameEntity entity, GameEntity entityInVicinity, HashSet<GameEntity> entitiesToUnnotice)
		{
			if (entity.vision.EntitiesNoticed.Contains(entityInVicinity.id.Id))
			{
				entitiesToUnnotice.Add(entityInVicinity);
				return;
			}

			bool isEnemy = !_friendshipResolver.AreFriends(entity, entityInVicinity);

			if (isEnemy)
			{
				FovArea enemyFov = _calculatedAreaAccessor.FetchVisibilityFov(entityInVicinity.position.Position, entity.vision.VisionRange);

				// cheat (it's really checking if current actor is in sight of enemy). todo: use expression area of enemy actor (below) instead?
				bool enemyIsInSight = enemyFov.Positions.Contains(entity.position.Position);
				if (enemyIsInSight)
				{
					bool noticeEnemy = true; // can be calculated based on enemy awareness of the entity
					if (noticeEnemy /* || entity.suspiciousness.Suspiciousness >= _gameConfig.SuspiciousnessForAware*/)
					{
						if (!entity.hasStimuli)
						{
							entity.AddStimuli(new List<Stimulus>());
						}

						Stimulus noticeStimulus = new Stimulus
						{
							Type = StimulusType.INotice,
							Position = entityInVicinity.position.Position,
							ObjectEntityId = entityInVicinity.id.Id
						};
						entity.stimuli.Stimuli.Add(noticeStimulus);
					}
				}
			}
		}
	}
}