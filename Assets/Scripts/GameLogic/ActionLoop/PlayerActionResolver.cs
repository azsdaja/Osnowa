namespace GameLogic.ActionLoop
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entities;
	using GameCore;
	using Osnowa.Osnowa.AI.Activities;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;

	public class PlayerActionResolver : IActionResolver
	{
		private readonly IGameConfig _gameConfig;
		private readonly IEntityDetector _entityDetector;
		private readonly IActionFactory _actionFactory;
		private readonly HashSet<Decision> _moveInputs;
		private readonly IFriendshipResolver _friendshipResolver;
		private readonly GameContext _context;
		private readonly IActivityInterruptor _activityInterruptor;

		public PlayerActionResolver(IGameConfig gameConfig, IEntityDetector entityDetector, 
			IActionFactory actionFactory, IFriendshipResolver friendshipResolver, GameContext context, IActivityInterruptor activityInterruptor)
		{
			_gameConfig = gameConfig;
			_entityDetector = entityDetector;
			_actionFactory = actionFactory;
			_friendshipResolver = friendshipResolver;
			_context = context;
			_activityInterruptor = activityInterruptor;

			_moveInputs = new HashSet<Decision>
			{
				Decision.MoveUpLeft, Decision.MoveUp, Decision.MoveUpRight,
				Decision.MoveLeft, Decision.MoveRight,
				Decision.MoveDownLeft, Decision.MoveDown, Decision.MoveDownRight
			};
		}

		public IGameAction GetAction(GameEntity entity)
		{
			IGameAction gameActionToReturn = null;

			IActivity activity = entity.hasActivity ? entity.activity.Activity : null;
			if (activity != null)
			{
				ActivityStep activityStep = activity.CheckAndResolveStep(entity);

				if (activityStep.State == ActivityState.FinishedSuccess)
				{
				}
				else if (activityStep.State == ActivityState.FinishedFailure)
				{
					_activityInterruptor.FailAndReplace(entity, activity, null);
				}
				return activityStep.GameAction;
			}

			Decision decision = _context.playerDecision.Decision;
			if (decision == Decision.None)
			{
				return null;
			}

			if (decision == Decision.PickUp)
			{
				gameActionToReturn = ResolveForPickUp(entity);
			}
			else if (decision == Decision.Drop)
			{
				gameActionToReturn = ResolveForDrop(entity);
			}
			else if (decision == Decision.Eat)
			{
				gameActionToReturn = ResolveForEat(entity);
			}
			else if (decision == Decision.Pass)
			{
				gameActionToReturn = _actionFactory.CreatePassAction(entity);
			}
			else if (decision == Decision.TakeItem1)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 0);
			}
			else if (decision == Decision.TakeItem2)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 1);
			}
			else if (decision == Decision.TakeItem3)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 2);
			}
			else if (decision == Decision.TakeItem4)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 3);
			}
			else if (decision == Decision.TakeItem5)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 4);
			}
			else if (decision == Decision.TakeItem6)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 5);
			}
			else if (decision == Decision.TakeItem7)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 6);
			}
			else if (decision == Decision.TakeItem8)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 7);
			}
			else if (decision == Decision.TakeItem9)
			{
				gameActionToReturn = CreateTakeFromInventoryActionIfPossible(entity, 8);
			}
			else if (_moveInputs.Contains(decision))
			{
				gameActionToReturn = ResolveForMove(entity, decision);
			}
			else if (decision == Decision.Custom0)
			{
				gameActionToReturn = _actionFactory.CreateLambdaAction(
					targetEntity =>
					{
						targetEntity.ReplaceIntegrity(targetEntity.integrity.MaxIntegrity, targetEntity.integrity.MaxIntegrity);
						return Enumerable.Empty<IActionEffect>();
					}, entity);
			}
			else if (decision == Decision.Custom1)
			{
				gameActionToReturn = _actionFactory.CreateLambdaAction(actionEntity =>
				{
					throw new Exception("test pawła 2");
				}, entity);
			}
			else if (decision == Decision.Custom2)
			{
				gameActionToReturn = ResolveForAlphaNumber(2, entity);
			}
			else if (decision == Decision.Custom3)
			{
				gameActionToReturn = ResolveForAlphaNumber(3, entity);
			}
			else if (decision == Decision.Custom4)
			{
				gameActionToReturn = ResolveForAlphaNumber(4, entity);
			}
			else if (decision == Decision.Custom5)
			{
				gameActionToReturn = ResolveForAlphaNumber(5, entity);
			}

			if (gameActionToReturn != null)
				_context.ReplacePlayerDecision(Decision.None, Position.Zero, Position.MinValue);

			return gameActionToReturn;
		}

		public IGameAction CreateTakeFromInventoryActionIfPossible(GameEntity entity, int index)
		{
			bool canSwapInventoryWithHands = entity.hasEntityHolder && entity.entityHolder.EntityHeld != Guid.Empty
			                                 && entity.hasInventory && entity.inventory.EntitiesInInventory[index] != Guid.Empty;
			if (canSwapInventoryWithHands)
			{
				return _actionFactory.CreateSwapHandWithInventoryAction(entity, index);
			}
			if (!entity.hasInventory || entity.inventory.EntitiesInInventory[index] == Guid.Empty)
					return null;

			return _actionFactory.CreateTakeFromInventoryAction(entity, index);
		}

		public IGameAction CreateTakeToInventoryActionIfPossible(GameEntity entity)
		{
			bool playerCanTakeToInventory = entity.hasEntityHolder && entity.entityHolder.EntityHeld != Guid.Empty && entity.hasInventory
								&& entity.inventory.EntitiesInInventory.Contains(Guid.Empty);
			if (!playerCanTakeToInventory) return null;
			GameEntity itemInHands = _context.GetEntityWithId(entity.entityHolder.EntityHeld);
			return itemInHands != null ? _actionFactory.CreateTakeToInventoryAction(entity) : null;
		}

		public IGameAction CreateDropActionIfPossible(GameEntity playerEntity)
		{
			GameEntity entityHeld = _context.GetEntityWithId(playerEntity.entityHolder.EntityHeld);
			if (entityHeld == null)
				return null;

            return _actionFactory.CreateDropAction(entityHeld, playerEntity);
        }

        private IGameAction ResolveForEat(GameEntity entity)
		{
			if (entity.hasEntityHolder && entity.entityHolder.EntityHeld != Guid.Empty)
			{
				GameEntity heldItem = _context.GetEntityWithId(entity.entityHolder.EntityHeld);
			}
			return null;
		}

		private IGameAction ResolveForAlphaNumber(int number, GameEntity entity)
		{
			if (number == 4 && (!entity.hasEntityHolder || entity.entityHolder.EntityHeld == Guid.Empty))
			{
				GameEntity neighbourToKidnap = _entityDetector.DetectEntities(entity.position.Position, 1).
					FirstOrDefault(a => a != entity);
				if (neighbourToKidnap != null)
				{
					return _actionFactory.CreateKindapAction(neighbourToKidnap, entity);
				}
			}

			return null;
		}

		protected IGameAction ResolveForMove(GameEntity entity, Decision decision)
		{
			Position actionVector = GetActionVector(decision);
			Position targetPosition = actionVector + entity.position.Position;
			GameEntity blockingEntityAtTargetPosition = _entityDetector.DetectEntities(targetPosition)
				.FirstOrDefault(e => e.isBlockingPosition);
			IGameAction gameActionToReturn;
			if (blockingEntityAtTargetPosition != null)
			{
				gameActionToReturn = _friendshipResolver.AreFriends(entity, blockingEntityAtTargetPosition) 
					? _actionFactory.CreateDisplaceAction(entity, blockingEntityAtTargetPosition) 
					: _actionFactory.CreateAttackAction(entity, blockingEntityAtTargetPosition);
			}
			else
			{
				gameActionToReturn = _actionFactory.CreateJustMoveAction(actionVector, entity);
			}
			return gameActionToReturn;
		}

		protected virtual IGameAction ResolveForPickUp(GameEntity entity)
		{
			if (!entity.hasEntityHolder)
			{
				return null;
			}
			if (entity.entityHolder.EntityHeld != Guid.Empty && entity.hasInventory && !entity.inventory.EntitiesInInventory.Contains(Guid.Empty))
			{
				return null;
			}
			GameEntity itemInHands = _context.GetEntityWithId(entity.entityHolder.EntityHeld);
			if (itemInHands != null)
			{
				return _actionFactory.CreateTakeToInventoryAction(entity);
			}

			List<GameEntity> eligibleItemsToPickUp = _entityDetector.DetectEntities(entity.position.Position)
				.Where(e => e != entity)
				.Where(e => e.isCarryable).ToList();
		    GameEntity itemToPickUp = eligibleItemsToPickUp.FirstOrDefault();
			if (itemToPickUp == null)
				return null;
			return _actionFactory.CreatePickUpAction(itemToPickUp, entity);
		}

		protected virtual IGameAction ResolveForDrop(GameEntity entity)
		{
			if (entity.hasEntityHolder && entity.entityHolder.EntityHeld != Guid.Empty)
			{
				GameEntity itemToDrop = _context.GetEntityWithId(entity.entityHolder.EntityHeld);
				var gameActionToReturn = _actionFactory.CreateDropAction(itemToDrop, entity);
				return gameActionToReturn;
			}
			return null;
		}

	    protected Position GetActionVector(Decision input)
		{
			Position actionVector = Position.Zero;
			if (input == Decision.MoveUp)
			{
				actionVector = new Position(0, 1);
			}
			else if (input == Decision.MoveLeft)
			{
				actionVector = new Position(-1, 0);
			}
			else if (input == Decision.MoveDown)
			{
				actionVector = new Position(0, -1);
			}
			else if (input == Decision.MoveRight)
			{
				actionVector = new Position(1, 0);
			}
			else if (input == Decision.MoveUpLeft)
			{
				actionVector = new Position(-1, 1);
			}
			else if (input == Decision.MoveUpRight)
			{
				actionVector = new Position(1, 1);
			}
			else if (input == Decision.MoveDownLeft)
			{
				actionVector = new Position(-1, -1);
			}
			else if (input == Decision.MoveDownRight)
			{
				actionVector = new Position(1, -1);
			}
			return actionVector;
		}
	}
}