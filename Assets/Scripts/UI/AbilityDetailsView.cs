namespace UI
{
	using GameLogic.AI.Model;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class AbilityDetailsView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _nameLabel;
		[SerializeField] private TextMeshProUGUI _keyCodeLabel;
		[SerializeField] private TextMeshProUGUI _descriptionLabel;
		[SerializeField] private Image _preview;
		[SerializeField] private Image _background;

		public void Initialize(Ability ability)
		{
			_nameLabel.text = ability.name;
			_keyCodeLabel.text = ability.KeyCode.ToString();
			_descriptionLabel.text = ability.Description;
			_preview.sprite = ability.Sprite;
			_background.color = ability.BackgroundColor;
		}
	}
}
