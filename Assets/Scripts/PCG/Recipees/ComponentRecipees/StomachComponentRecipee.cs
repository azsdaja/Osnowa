namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Stomach", menuName = "Kafelki/Entities/Recipees/Stomach", order = 0)]
	public class StomachComponentRecipee : ComponentRecipee
	{
		public int InitialSatiation = 200;
		public int MaxSatiation = 300;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceStomach(InitialSatiation, MaxSatiation);
		}
	}
}