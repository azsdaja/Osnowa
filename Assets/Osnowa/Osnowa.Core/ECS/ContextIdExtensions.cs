using System;
using Entitas;

namespace Osnowa.Osnowa.Core.ECS
{
	public static class ContextsIdExtensions
	{
		public static void SubscribeId(this Contexts contexts)
		{
			foreach (var context in contexts.allContexts)
			{
				if (Array.FindIndex(context.contextInfo.componentTypes, v => v == typeof(IdComponent)) >= 0)
				{
					context.OnEntityCreated -= AddId;
					context.OnEntityCreated += AddId;
				}
			}
		}

		public static void AddId(IContext context, IEntity entity)
		{
			(entity as GameEntity).ReplaceId(Guid.NewGuid());
		}
	}
}