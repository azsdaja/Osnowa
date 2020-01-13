namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.RNG;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Sex", menuName = "Kafelki/Entities/Recipees/Sex", order = 0)]
	public class SexComponentRecipee : ComponentRecipee
	{
		public float MaleChance;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			bool isMale = rng.Check(MaleChance);
			entity.ReplaceSex(isMale);
		}
	}
}