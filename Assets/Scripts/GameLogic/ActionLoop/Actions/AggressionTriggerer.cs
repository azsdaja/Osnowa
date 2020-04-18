namespace GameLogic.ActionLoop.Actions
{
	using System;
	using Activities;
	using Osnowa.Osnowa.AI.Activities;
	using UI;
	using UnityEngine;

	class AggressionTriggerer : IAggressionTriggerer
	{
		private readonly IUiFacade _uiFacade;
		private readonly IPositionEffectPresenter _positionEffectPresenter;

		public AggressionTriggerer(IUiFacade uiFacade, IPositionEffectPresenter positionEffectPresenter)
		{
			_uiFacade = uiFacade;
			_positionEffectPresenter = positionEffectPresenter;
		}

		public void TriggerAggressionIfEligible(GameEntity target)
		{
			if (target.isPlayerControlled) return;

			try
			{
				// interrupting current activity
				if (target.hasActivity)
				{
					IActivity activity = target.activity.Activity;
					if(!(activity is AttackActivity))
						target.RemoveActivity();
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
				throw;
			}

			if (target.isAggressive) return;

			target.isAggressive = true;
			_uiFacade.AddLogEntry($"{target.view.Controller.Name} becomes aggressive!");
			_positionEffectPresenter.ShowPositionEffect(target.position.Position, "!", Color.red);
			if (target.hasActivity)
			{
				target.RemoveActivity();
			}
		}

	}
}