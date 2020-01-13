using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Statuses
{
	public abstract class StatusComponent : IComponent
	{
		public int TurnsLeft;

		// would be cool to have:
		// public static abstract bool ResolvePresence(GameEntity entity);
		// public static abstract bool DecreaseCounter(GameEntity entity);
		// public static abstract void Remove(GameEntity entity);,
		// but static abstract is not handled by the language
	}
}