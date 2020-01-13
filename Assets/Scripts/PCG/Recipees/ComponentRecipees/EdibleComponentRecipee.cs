namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.RNG;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Edible", menuName = "Kafelki/Entities/Recipees/Edible", order = 0)]
	public class EdibleComponentRecipee : ComponentRecipee
	{
		public int Satiety;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceEdible(Satiety);
		}
	}
}