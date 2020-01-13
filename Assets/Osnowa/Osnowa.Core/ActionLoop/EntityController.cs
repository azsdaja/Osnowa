namespace Osnowa.Osnowa.Core.ActionLoop
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GameLogic.ActionLoop.Actions;
	using UnityEngine;
	using Zenject;

	/// <summary>
	/// Updates actors' control data, resolves their actions and executes the actions and their effects.
	/// Energy-based control flow inspired by Robert Nyström's article: 
	/// http://journal.stuffwithstuff.com/2014/07/15/a-turn-based-game-loop/
	/// </summary>
	public class EntityController : IEntityController
	{
		private readonly IActionResolver _actionResolver;

		public EntityController([Inject(Id = "_actionResolver")] IActionResolver actionResolver)
		{
			_actionResolver = actionResolver;
		}

		public bool GiveControl(GameEntity entity)
		{
			// todo: maybe instead when an actor is blocked, he should resolve his action 
			// and store it and don't execute it until ready
			if (entity.hasBlockedUntil && DateTime.UtcNow < entity.blockedUntil.BlockedUntil)
			{
				return false;
			}

			IGameAction gameAction;
			try
			{
				gameAction = _actionResolver.GetAction(entity);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message + ", stack trace: "+ e.StackTrace);
				gameAction = new LambdaAction(entity, 1f, null, gameEntity => Enumerable.Empty<IActionEffect>());
			}

			bool gameActionIsResolved = gameAction != null;
			if (gameActionIsResolved)
			{
//				Debug.Log($"{entity.view.Controller.Name} has action: {gameAction.GetType().Name}");

				PerformTurn(entity, gameAction);
				return true;
			}

			return false;
		}

		private static void PerformTurn(GameEntity entity, IGameAction gameAction)
		{
			float newEnergy = entity.energy.Energy - gameAction.EnergyCost;
			gameAction.Entity.ReplaceEnergy(entity.energy.EnergyGainPerSegment, newEnergy);

			try
			{
				IEnumerable<IActionEffect> effects = gameAction.Execute();
				foreach (IActionEffect effect in effects)
				{
					effect.Process();
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message + ", stack trace: "+ e.StackTrace);
			}
		}
	}
}