using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UiElementSelector : MonoBehaviour, IUiElementSelector
{
	private Image _image;
	private RectTransform _rectTransform;
	private IUiManager _uiManager;

	public int Padding = 16;

	[Inject]
	public void Init(IUiManager uiFacade)
	{
		_uiManager = uiFacade;
	}

	void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_image = GetComponent<Image>();
		_image.enabled = false;
	}

	public void Select(RectTransform parent)
	{
		gameObject.SetActive(true);

		_rectTransform = GetComponent<RectTransform>();

		_rectTransform.SetParent(parent);
		StretchAndAddPadding(_rectTransform, parent);

		_image.enabled = true;
	}

	public void Deselect()
	{
		transform.SetParent(_uiManager.UiElementsParent);
		_image.enabled = false;
	}

	private void StretchAndAddPadding(RectTransform rectTransform, RectTransform parent)
	{
		rectTransform.anchoredPosition = parent.position;
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(1, 1);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.sizeDelta = parent.rect.size;

		rectTransform.offsetMin = new Vector2(-Padding, -Padding);
		rectTransform.offsetMax = new Vector2(Padding, Padding);
	}
}