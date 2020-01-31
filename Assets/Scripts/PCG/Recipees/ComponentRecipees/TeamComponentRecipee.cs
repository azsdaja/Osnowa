namespace PCG.Recipees.ComponentRecipees
{
	using Osnowa.Osnowa.Rng;
	using Osnowa.Unassigned;
	using UnityEngine;

	[CreateAssetMenu(fileName = "Team", menuName = "Osnowa/Entities/Recipees/Team", order = 0)]
	public class TeamComponentRecipee : ComponentRecipee
	{
		public Team Team;

		public override void ApplyToEntity(GameEntity entity, IRandomNumberGenerator rng)
		{
			entity.ReplaceTeam(Team);
		}
	}
}