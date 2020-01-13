using System;

namespace Osnowa.Osnowa.Core.ECS
{
	public static class GameContextExtensions
	{
		public static GameEntity GetPlayerEntity(this GameContext context)
		{
			if (!context.hasPlayerEntity || context.playerEntity.Id == Guid.Empty)
				return null;
			return context.GetEntityWithId(context.playerEntity.Id);
		}
	}
}