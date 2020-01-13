namespace PCG.MapIngredientConfigs
{
	using System;
	using UnityEngine;

	[CreateAssetMenu(fileName = "MapIngredientConfig", menuName = "Kafelki/PCG/Maps/MapIngredientConfig", order = 0)]
	public class MapIngredientConfig : ScriptableObject
	{
		public event Action OnUpdate;
		public AnimationCurve ValueTweak = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		public Gradient ValueToColor;
		public MapIngredientPresentationMode PresentationMode;
		public string TextForBuildingMap;

		private DateTime _startedCountDown;
		private float _refreshInterval = 1f;
		private bool _countdownToRefresh;

		public void OnValidate()
		{
			if (OnUpdate == null)
				return;

			if (_countdownToRefresh == false)
			{
				_countdownToRefresh = true;
				_startedCountDown = DateTime.UtcNow;
			}

			bool intervalHasPassed = DateTime.UtcNow - _startedCountDown > TimeSpan.FromSeconds(_refreshInterval);

			if (intervalHasPassed)
			{
				OnUpdate();
				_countdownToRefresh = false;
			}
		}
	}
}