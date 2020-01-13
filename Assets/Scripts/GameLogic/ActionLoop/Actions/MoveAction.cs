namespace GameLogic.ActionLoop.Actions
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Grid;

	public class MoveAction : DirectedAction
	{
		private readonly IGrid _grid;

		public MoveAction(GameEntity entity, float energyCost, IActionEffectFactory actionEffectFactory, 
			Position direction, IGrid grid) 
			: base(entity, energyCost, actionEffectFactory, direction)
		{
			GuardDirection(direction);
			_grid = grid;
		}

		public override IEnumerable<IActionEffect> Execute()
		{
			Position previousPosition = Entity.position.Position;
			Position newPosition = previousPosition + Direction;
			if (_grid.IsWalkable(newPosition))
			{
				IActionEffect effect = ActionEffectFactory.CreateMoveEffect(Entity, previousPosition);
				Entity.ReplacePosition(newPosition);
				yield return effect;
			}
			else
			{
				IActionEffect effect = ActionEffectFactory.CreateBumpEffect(Entity, newPosition);
				yield return effect;
			}
		}

		private void GuardDirection(Position direction)
		{
			if(direction.x > 1 || direction.x < -1 || direction.y > 1 || direction.y < -1)
				throw new ArgumentException("Direction to move is exceeding one step: " + direction, nameof(direction));
		}
	}
}