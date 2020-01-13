using System;
using C5;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private  ScrollRect _scrollRect;
	private CircularQueue<string> _queue;

	public int MaxChars = 800;

	// Use this for initialization
	void Start ()
	{
		_text.text = "";
	}

	public void AddEntry(string entry)
	{
		if (_text != null)
		{
			string finalText = _text.text + Environment.NewLine + entry;
			if (finalText.Length > MaxChars)
			{
				const int carsToRemove = 200;
				finalText = finalText.Substring(carsToRemove);
			}
			_text.text = finalText;
			_scrollRect.verticalNormalizedPosition = 0.0f;
		}
	}
}