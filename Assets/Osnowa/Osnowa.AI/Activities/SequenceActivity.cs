namespace Osnowa.Osnowa.AI.Activities
{
	using System.Collections.Generic;

	public class SequenceActivity : Activity
	{
		private readonly IEnumerator<IActivity> _activityEnumerator;
		private IActivity _subactivity;

		public SequenceActivity(IEnumerator<IActivity> activities, string name) : base(name)
		{
			_activityEnumerator = activities;
			_activityEnumerator.MoveNext();
			_subactivity = _activityEnumerator.Current;
		}

		public override string GetFullName()
		{
			return Name + " / " + _activityEnumerator.Current?.GetFullName();
		}

		public override ActivityStep ResolveStep(GameEntity entity)
		{
			ActivityStep stepFromSubActivity = _subactivity.CheckAndResolveStep(entity);

			if (stepFromSubActivity.State == ActivityState.FinishedSuccess)
			{
				if (_activityEnumerator.MoveNext())
				{
					_subactivity = _activityEnumerator.Current;

					return new ActivityStep
					{
						State = ActivityState.InProgress,
						GameAction = stepFromSubActivity.GameAction
					};
				}
			}

			return stepFromSubActivity;
		}
	}
}