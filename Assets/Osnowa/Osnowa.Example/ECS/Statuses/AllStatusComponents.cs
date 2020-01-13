using System;
using Entitas;
using Osnowa.Osnowa.Example.ECS.Senses;

namespace Osnowa.Osnowa.Example.ECS.Statuses
{
	public static class AllStatusComponents
	{
		public static Type[] Types()
		{
			return new[]{typeof(AwareComponent), typeof(SleepingComponent)};
		}

		public static IAnyOfMatcher<GameEntity> Matcher()
		{
			return GameMatcher.AnyOf(GameMatcher.Aware, GameMatcher.Sleeping);
		}

		public static bool Conditions(GameEntity entity)
		{
			return entity.hasAware || entity.hasSleeping;
		}
	}
}