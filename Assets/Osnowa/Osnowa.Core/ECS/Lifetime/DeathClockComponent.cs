using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Lifetime
{
    /// <summary>
    /// Destroys the holding entity after given amount of turns has passed.
    /// </summary>
	public class DeathClockComponent : IComponent
	{
		public int TurnsLeft;
	}
}