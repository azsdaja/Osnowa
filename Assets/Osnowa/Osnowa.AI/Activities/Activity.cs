namespace Osnowa.Osnowa.AI.Activities
{
	using System;
	using Core.ActionLoop;

	public abstract class Activity : IActivity
	{
		private readonly Predicate<GameEntity> _isStillValid;

		protected Activity(string name, Predicate<GameEntity> isStillValid = null)
		{
			_isStillValid = isStillValid ?? (entity => true);
			Name = name;
		}

		public ActivityStep CheckAndResolveStep(GameEntity entity)
		{
			bool shouldInterrupt = !_isStillValid(entity);
			return shouldInterrupt ? Fail(entity) : ResolveStep(entity);
		}

		public abstract ActivityStep ResolveStep(GameEntity entity);

		public string Name { get; }
		public float Score { get; set; }

		public virtual string GetFullName()
		{
			return Name;
		}

		public virtual void OnStart(GameEntity entity)
		{
		}

		public virtual void OnSuccess(GameEntity entity)
		{
		}

		public virtual void OnFailure(GameEntity entity)
		{
		}

		protected virtual ActivityStep Fail(GameEntity entity, IGameAction lastAction = null)
		{
			OnFailure(entity);

			return new ActivityStep
			{
				State = ActivityState.FinishedFailure,
				GameAction = lastAction
			};
		}

		protected virtual ActivityStep Succeed(GameEntity entity, IGameAction lastAction = null)
		{
			OnSuccess(entity);

			return new ActivityStep
			{
				State = ActivityState.FinishedSuccess,
				GameAction = lastAction
			};
		}
	}
}