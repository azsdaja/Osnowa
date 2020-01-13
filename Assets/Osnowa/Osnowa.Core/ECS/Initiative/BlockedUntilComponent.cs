using System;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS.Initiative
{
    /// <summary>
    /// Used for blocking an entity for executing its action until some point in time, so that tasks like animation can be finished.
    /// </summary>
	public class BlockedUntilComponent : IComponent
	{
		public DateTime BlockedUntil;
	}
}