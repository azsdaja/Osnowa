using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Presence
{
	using Core;

	/// <summary>
	/// Position on the game grid after last turn of given entity.
	/// </summary>
	[Game]
	public class PositionAfterLastTurnComponent : IComponent
	{
		public Position Position { get; set; }
	}
}