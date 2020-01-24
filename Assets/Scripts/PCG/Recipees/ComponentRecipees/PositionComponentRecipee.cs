namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Position", menuName = "Kafelki/Entities/Recipees/Position", order = 0)]
	public class PositionComponentRecipee : ComponentRecipee
	{
		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplacePosition(new Position());
		}
	}
}