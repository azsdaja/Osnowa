namespace GameLogic
{
	using System;
	using System.Collections.Generic;
	using AI.Model;
	using Osnowa;

	/// <summary>
	/// A facade for functions and interfaces responsible for manipulating UI.
	/// </summary>
	/// <remarks>
	/// Rule of thumb: if only one method from an object is used, forward it as a method;
	/// otherwise, expose object as an interface instance.
	/// </remarks>
	public interface IUiFacade
	{
		void SetHoveredPositionText(string text);
		void ChangeAbilityAccesibility(Ability ability, bool present, bool usable = true);
		void SelectAbility(Ability ability);
		void ShowEntityDetails(GameEntity entity, GameEntity potentialEntityInTool = null, bool atFeet = false);
	    void ShowFloodNumbers(IFloodArea floodArea);
		IUiElementSelector UiElementSelector { get; }
		void AddLogEntry(string logEntry, LogEntryType type = LogEntryType.Plain);
		void RefreshInventory(List<Guid> entitiesInInventory);
		void SetHealth(int health, int maxHealth);
	    void HandlePlayerDeath(string deathMessage);
	    void ShowAbilityDetails(Ability ability);
	}
}