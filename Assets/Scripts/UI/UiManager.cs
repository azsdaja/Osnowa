namespace UI
{
	using System;
	using System.Collections.Generic;
	using GameLogic;
	using GameLogic.AI.Model;
	using Osnowa;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Grid;
	using TMPro;
	using UnityEngine;
	using Zenject;

	public class UiManager : MonoBehaviour, IUiManager
	{
		[SerializeField] private GenericBar _satiationBar;
		[SerializeField] private TextMeshProUGUI _hoveredTerrainText;
		[SerializeField] private AbilitiesGroup _contextualAbilitiesGroup;
		[SerializeField] private AbilitiesGroup _nonContextualAbilitiesGroup;
		[SerializeField] private EntityDetailsView _entityDetailsView;
		[SerializeField] private NumbersAreaPresenter _numbersAreaPresenter;
		[SerializeField] private Log _log;
		[SerializeField] private UiElementSelector _uiElementSelector;
		[SerializeField] private List<InventoryItemPreview> _inventoryItems;
		[SerializeField] private TextMeshProUGUI _strengthIndicator;
		[SerializeField] private TextMeshProUGUI _vitalityIndicator;
		[SerializeField] private TextMeshProUGUI _healthIndicator;
		[SerializeField] private GameObject _quitMenu;
		[SerializeField] private GameObject _youDiePanel;
		[SerializeField] private TextMeshProUGUI _youDieText;

		private IOsnowaContextManager _contextManager;
		private IEntityDetector _entityDetector;

		[Inject]
		public void Init(IOsnowaContextManager contextManager, IEntityDetector entityDetector)
		{
			_contextManager = contextManager;
			_entityDetector = entityDetector;
		}


		public void SetSatiation(int stomachSatiation, int stomachMaxSatiation)
		{
			_satiationBar.OnChanged(stomachSatiation / (float)stomachMaxSatiation);
		}

		public void SetHoveredPositionText(string text)
		{
			if (text == null)
			{
				_hoveredTerrainText.text = string.Empty;
				return;
			}

			_hoveredTerrainText.text = text;
		}

		public void ChangeAbilityAccesibility(Ability ability, bool present, bool usable = true)
		{
			AbilitiesGroup abilityGroupToChange = ability.IsContextual ? _contextualAbilitiesGroup : _nonContextualAbilitiesGroup;
			abilityGroupToChange.SetAbility(ability, present, usable ? AbilityViewState.Usable : AbilityViewState.Unusable);
		}

		public void SelectAbility(Ability ability)
		{
			_contextualAbilitiesGroup.SelectAbility(ability);
			_nonContextualAbilitiesGroup.SelectAbility(ability);
		}

		public void ShowEntityDetails(GameEntity entity, GameEntity potentialEntityInTool = null, bool atFeet = false)
	    {
            if(entity != null)
	        {
	            _entityDetailsView.gameObject.SetActive(true);
	            _entityDetailsView.Initialize(entity);
	        }
            else
                _entityDetailsView.gameObject.SetActive(false);
        }

        public void ShowFloodNumbers(IFloodArea floodArea)
		{
			_numbersAreaPresenter.Show(floodArea);
		}

		public IUiElementSelector UiElementSelector => _uiElementSelector;

		public void AddLogEntry(string logEntry, LogEntryType type = LogEntryType.Plain)
		{
			_log.AddEntry(logEntry);
		}

	    public void RefreshInventory(List<Guid> entitiesInInventory)
		{
			for (int index = 0; index < entitiesInInventory.Count; index++)
			{
				Guid guid = entitiesInInventory[index];
				GameEntity entity = Contexts.sharedInstance.game.GetEntityWithId(guid);
				_inventoryItems[index].ItemEntity = entity;
			    if (entity == null)
			    {
			        _inventoryItems[index].Image.sprite = null;
			        _inventoryItems[index].Image.enabled = false;
			    }
			    else
			    {
			        _inventoryItems[index].Image.sprite = entity.view.Controller.GetSprite();
			        _inventoryItems[index].Image.preserveAspect = true;
			        _inventoryItems[index].Image.enabled = true;
			    }
			}
		}

		public void SetStrength(int strengthValue)
		{
			_strengthIndicator.text = strengthValue.ToString();
		}

		public void SetVitality(int vitalityValue)
		{
			_vitalityIndicator.text = vitalityValue.ToString();
		}

		public void SetHealth(int health, int maxHealth)
		{
			_healthIndicator.text = health + "/" + maxHealth;

			float integrity = maxHealth == 0 ? 0f : (float)health / maxHealth;

			_healthIndicator.color = integrity > 0.6f
				? Color.white
				: integrity > 0.25f ? Color.yellow : new Color(1f, 0.15f, 0.15f);
		}

	    public void HandlePlayerDeath(string deathMessage)
		{
			_quitMenu.SetActive(true);
			_youDiePanel.SetActive(true);
			_youDieText.text = deathMessage;
		}

	    public Transform UiElementsParent => transform;
	}
}