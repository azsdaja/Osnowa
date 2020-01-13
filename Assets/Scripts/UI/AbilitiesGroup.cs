namespace UI
{
	using System.Linq;
	using GameLogic;
	using GameLogic.AI.Model;
	using UnityEngine;
	using UnityUtilities;
	using Zenject;

	public class AbilitiesGroup : MonoBehaviour
	{
		private readonly AbilityView[] _abilityViewsByOrder = new AbilityView[100];
		private IUiFacade _uiFacade;

		[Inject]
		public void Init(IUiFacade uiFacade)
		{
			_uiFacade = uiFacade;
		}

		public void SetAbility(Ability ability, bool shouldBePresent, AbilityViewState newState)
		{
			bool alreadyPresent = _abilityViewsByOrder[ability.Order] != null;
			if (alreadyPresent && !shouldBePresent)
			{
				AbilityView viewToRemove = _abilityViewsByOrder[ability.Order];
				_abilityViewsByOrder[ability.Order] = null;
				PoolingManager.Free(PoolingManager.AbilityView, viewToRemove.gameObject);
			}
			else if (!alreadyPresent && shouldBePresent)
			{
				AbilityView newView = PoolingManager.Fetch<AbilityView>(PoolingManager.AbilityView, Vector3.zero,
					Quaternion.identity, transform);
				_abilityViewsByOrder[ability.Order] = newView;
				newView.InitializeWith(ability, newState);

				AdjustViewOrder(ability, newView);
			}
			else if(alreadyPresent)
			{
				AbilityView viewToChange = _abilityViewsByOrder[ability.Order];
				viewToChange.UpdateForState(newState);
			}
		}

		public void SelectAbility(Ability ability)
		{
			AbilityView abilityView = _abilityViewsByOrder[ability.Order];
			if (abilityView == null || abilityView.State != AbilityViewState.Usable)
				return;

			RectTransform parentForSelection = abilityView.GetComponent<RectTransform>();
			if (parentForSelection == null)
				return;

			_uiFacade.UiElementSelector.Select(parentForSelection);
		}

		public void DeselectAbility(Ability ability)
		{
			_uiFacade.UiElementSelector.Deselect();
		}

		private void AdjustViewOrder(Ability ability, AbilityView newView)
		{
			AbilityView[] children = transform.GetComponentsInChildren<AbilityView>();
			AbilityView firstViewWithBiggerIndex = children.FirstOrDefault(c => c.Ability.Order > ability.Order);
			if (firstViewWithBiggerIndex != null)
			{
				newView.transform.SetSiblingIndex(firstViewWithBiggerIndex.transform.GetSiblingIndex());
			}
		}
	}
}