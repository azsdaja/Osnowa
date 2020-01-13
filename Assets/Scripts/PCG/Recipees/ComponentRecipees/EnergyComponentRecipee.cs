namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.RNG;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Energy", menuName = "Kafelki/Entities/Recipees/Energy", order = 0)]
	public class EnergyComponentRecipee : ComponentRecipee
	{
		public float EnergyGain = 0.1f;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceEnergy(EnergyGain, rng.NextFloat());
		}
	}
}