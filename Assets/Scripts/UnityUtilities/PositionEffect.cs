namespace UnityUtilities
{
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// A label with limited life span displayed at given position. Dedicated mostly for debugging purposes.
	/// </summary>
	public class PositionEffect : MonoBehaviour
	{
		private Color _initialColor;
		private Color _initialColorButTransparent;

		private float _duration;
		private float _timePassed;

		[SerializeField] private Image _selection;
		[SerializeField] private TextMeshProUGUI _text;

		private GameObject _positionEffectPrefab;

		public void Initialize(string text, float duration = 1f, bool markPosition = false)
		{
			Initialize(text, Color.white, duration, markPosition);
		}

		public void Initialize(string text, Color color, float duration = 1f, bool markPosition = false)
		{
			_initialColor = color;

			_text.text = text;
			_text.transform.localPosition = Vector3.up;
			_text.color = _initialColor;

			_selection.gameObject.SetActive(markPosition);
			_selection.color = _initialColor;
			_selection.transform.localPosition = Vector3.zero;

			_initialColorButTransparent = new Color(color.r, color.g, color.b, 0);
			_duration = duration;
			_timePassed = 0f;
		}

		void Update()
		{
			_timePassed += Time.unscaledDeltaTime;
			float progress = _timePassed / _duration;
			if (progress >= 1f)
			{
				PoolingManager.Free(PoolingManager.PositionEffect, gameObject);
				return;
			}

			var currentColor = ColorUtilities.Lerp3(_initialColor, _initialColor, _initialColorButTransparent, progress);
			
			_text.color = currentColor;
			
			_text.transform.Translate(0f, progress * Time.deltaTime, 0f);

			_selection.color = currentColor;

		}
	}
}