using Entitas;

namespace Osnowa.Osnowa.Example.ECS.AI
{
	using Core.ActionLoop;

	/// <summary>
    /// Stores action that an entity has resolved and is waiting to be able to execute it.
    /// </summary>
	public class PreparedActionComponent : IComponent
	{
		public GameAction Action;
	}
}