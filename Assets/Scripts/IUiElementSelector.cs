using UnityEngine;

public interface IUiElementSelector
{
	void Select(RectTransform parent);
	void Deselect();
}