using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Osnowa.Osnowa.Grid;

namespace Osnowa.Osnowa.Example.ECS.Abilities
{
	using GameLogic;

	public class ResolveAbilitiesPerTurnSystem : ReactiveSystem<GameEntity>
	{
		private readonly IEntityDetector _entityDetector;
		private readonly IUiFacade _uiFacade;
		private readonly IGameConfig _gameConfig;
		private readonly GameContext _context;

		public ResolveAbilitiesPerTurnSystem(GameContext context, IEntityDetector entityDetector, IUiFacade uiFacade, IGameConfig gameConfig)
			: base(context)
		{
			_context = context;
			_entityDetector = entityDetector;
			_uiFacade = uiFacade;
			_gameConfig = gameConfig;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.AllOf(GameMatcher.PlayerControlled, /*GameMatcher.InitiativeOwner, */GameMatcher.ExecutePreTurn));
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.isPlayerControlled && entity.isExecutePreTurn;
		}

        protected override void Execute(List<GameEntity> entities)
	    {
	        GameEntity playerEntity = entities.SingleEntity();

	        _uiFacade.ChangeAbilityAccesibility(_gameConfig.Abilities.SpawnActors, true);

	        List<GameEntity> entitiesAround = _entityDetector.DetectEntities(playerEntity.position.Position, 1).ToList();

	        bool pickUpOrPutInIsPossible = false;
	        bool dropIsPossible = false;

	        if (playerEntity.hasEntityHolder)
	        {
	            bool isHoldingSomething = playerEntity.entityHolder.EntityHeld != Guid.Empty;
	            dropIsPossible = isHoldingSomething;
	            pickUpOrPutInIsPossible = isHoldingSomething && playerEntity.hasInventory &&
	                                      playerEntity.inventory.EntitiesInInventory.Any(e => e == Guid.Empty);
	        }

	        List<GameEntity> entitiesAtPosition = _entityDetector.DetectEntities(playerEntity.position.Position)
	            .Where(e => e != playerEntity && !e.hasHeld).ToList();

            GameEntity anyEntityAtFeet = entitiesAtPosition.FirstOrDefault();
            _uiFacade.ShowEntityDetails(anyEntityAtFeet, atFeet: true);

            if (playerEntity.entityHolder.EntityHeld == Guid.Empty)
	        {
	            GameEntity carryableEntityAtFeet = entitiesAtPosition.FirstOrDefault(e => e.isCarryable);
	            pickUpOrPutInIsPossible |= carryableEntityAtFeet != null;
	        }

	        _uiFacade.ChangeAbilityAccesibility(_gameConfig.Abilities.Drop, dropIsPossible);
	        _uiFacade.ChangeAbilityAccesibility(_gameConfig.Abilities.PickUp, pickUpOrPutInIsPossible);
	    }
	}
}