namespace PCG.Recipees.ComponentRecipees
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Vision", menuName = "Osnowa/Entities/Recipees/Vision", order = 0)]
	public class VisionComponentRecipee : ComponentRecipee
	{
		public int VisionRay;
		public int PerceptionRay;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceVision(VisionRay, PerceptionRay, new HashSet<Guid>());
		}
	}
}