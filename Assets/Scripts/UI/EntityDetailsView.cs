namespace UI
{
	using Osnowa.Osnowa.Context;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;
	using Zenject;

	public class EntityDetailsView : MonoBehaviour
	{
		private IObjectToHashColorCalculator _objectToHashColorCalculator;

		[SerializeField] private TextMeshProUGUI _nameLabel;
		[SerializeField] private Image _preview;
		[SerializeField] private Image _background;
		private Color _initialColor;
		private Color _previewColor;
		[SerializeField] private LayoutElement _propertiesParent;	
		[SerializeField] private LayoutElement _statsParent;	
		[SerializeField] private TextMeshProUGUI _strength;	
		[SerializeField] private TextMeshProUGUI _vitality;	
		[SerializeField] private TextMeshProUGUI _health;	
		private IOsnowaContextManager _contextManager;
		private bool _colorsInitialized;

		[Inject]
		public void Init(IObjectToHashColorCalculator objectToHashColorCalculator, IOsnowaContextManager contextManager)
		{
			_objectToHashColorCalculator = objectToHashColorCalculator;
			_contextManager = contextManager;
		}

		void InitializeColors()
		{
			_initialColor = _background.color;
			_previewColor = _background.color*1.2f;
			_colorsInitialized = true;
		}

		public void Initialize(GameEntity entity, bool isPreview = false)
		{
			if (!_colorsInitialized)
			{
				InitializeColors();	
			}

			_background.color = isPreview ? _previewColor : _initialColor;

			if (entity.hasView)
			{
				string ownName = entity.view.Controller.Name;
				_nameLabel.text = ownName;

				Sprite sprite = entity.view.Controller.GetSprite();
				_preview.sprite = sprite;
			}

			if (entity.hasIntegrity)
			{
				_health.text = entity.integrity.Integrity + "/" + entity.integrity.MaxIntegrity;
			}
		}
	}
}
