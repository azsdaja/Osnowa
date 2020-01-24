namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.NaPozniej;
	using Osnowa.Osnowa.Rng;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Team", menuName = "Kafelki/Entities/Recipees/Team", order = 0)]
	public class TeamComponentRecipee : ComponentRecipee
	{
		public Team Team;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceTeam(Team);
		}
	}
}