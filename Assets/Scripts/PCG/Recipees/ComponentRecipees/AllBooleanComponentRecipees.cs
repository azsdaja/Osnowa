namespace PCG.Recipees.ComponentRecipees
{
	using System;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "AllBooleanComponentRecipees", menuName = "Kafelki/Entities/Recipees/AllBooleanComponentRecipees", order = 0)]
	public class AllBooleanComponentRecipees : ComponentRecipee
	{
		public bool IsBlockingPosition;
		public bool IsCarryable;
		public bool HasEntityHolder;
		public bool IsAggressive;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.isBlockingPosition = IsBlockingPosition;
			entity.isCarryable = IsCarryable;
			entity.isAggressive = IsAggressive;

			if (HasEntityHolder)
				entity.ReplaceEntityHolder(Guid.Empty);

		}
	}
}