namespace UnityUtilities
{
	using UnityEngine;

	public class InputWithRepeating : IInputWithRepeating
	{
		private readonly float _initialTimeLeftToRepeat;
		private readonly float _repeatInterval;
		private float _timeLeftToRepeat;

		public InputWithRepeating(float initialTimeLeftToRepeat = .35f, float repeatInterval = 0.06f)
		{
			_initialTimeLeftToRepeat = initialTimeLeftToRepeat;
			_repeatInterval = repeatInterval;
			_timeLeftToRepeat = _initialTimeLeftToRepeat;
		}

		public bool KeyDownOrRepeating(KeyCode keyCode)
		{
			return Input.GetKeyDown(keyCode) || GetAndUpdateKey(keyCode);
		}

		public bool GetAndUpdateKey(KeyCode keyCode)
		{
			if (Input.GetKey(keyCode))
			{
				_timeLeftToRepeat -= Time.unscaledDeltaTime;
				if (_timeLeftToRepeat < 0)
				{
					_timeLeftToRepeat = _repeatInterval;
					return true;
				}
			}
			return false;
		}

		public void ResetTime()
		{
			_timeLeftToRepeat = _initialTimeLeftToRepeat;
		}
	}
}