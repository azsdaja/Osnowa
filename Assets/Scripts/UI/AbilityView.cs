namespace UI
{
	using GameLogic;
	using GameLogic.AI.Model;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;
	using Zenject;

	public class AbilityView : MonoBehaviour
	{
		public class Factory : PlaceholderFactory<AbilityView>
		{
		}

		private IUiFacade _uiFacade;

		public Ability Ability;
		public Image Frame;
		public Image Icon;
		public TextMeshProUGUI Label;
		public AbilityViewState State { get; private set; }

		[Inject]
		public void Init(IUiFacade uiFacade)
		{
			_uiFacade = uiFacade;
		}

		public void UpdateForState(AbilityViewState newState)
		{
			if (newState == State)
				return;
			State = newState;

			if (newState == AbilityViewState.Unusable)
			{
				Frame.color *= Color.gray;
				Icon.color *= Color.gray;
				Label.color *= Color.gray;
			}
			else if (newState == AbilityViewState.Usable)
			{
				Frame.color = Ability.BackgroundColor;
				Icon.color = Color.white;
				Label.color = Color.white;
			}
		}

		public void InitializeWith(Ability ability, AbilityViewState initialState)
		{
			Ability = ability;

			string keyCodeAsString = Ability.KeyCode.ToString();
			string label = keyCodeAsString.StartsWith("Alpha") ? keyCodeAsString.Substring(5) : keyCodeAsString;
			Label.text = label;
			Frame.color = Ability.BackgroundColor;
			Icon.sprite = Ability.Sprite;

			UpdateForState(initialState);
		}

		public void OnMouseEnter()
		{
			_uiFacade.ShowAbilityDetails(Ability);
		}

		public void OnMouseExit()
		{
			_uiFacade.ShowAbilityDetails(null);
		}
	}
}
