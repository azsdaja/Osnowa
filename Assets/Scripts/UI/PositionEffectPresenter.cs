namespace UI
{
	using System.Collections;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;
	using UnityUtilities;

	public class PositionEffectPresenter : IPositionEffectPresenter
	{
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;
		private PositionEffect _positionEffectPrefab;

		public PositionEffectPresenter(IUnityGridInfoProvider unityGridInfoProvider)
		{
			_unityGridInfoProvider = unityGridInfoProvider;
		}

		public PositionEffect ShowPositionEffect(Position position, string text, Color? color = null, bool markPosition = false, float duration = 2.5f, float delay = 0.0f)
		{
			PositionEffect positionEffect = FetchPositionEffectAtPosition(position);

			Color effectColor = color ?? Color.white;

			if (delay > 0f)
			{
				positionEffect.StartCoroutine(InitializePositionEffectWithDelay(positionEffect, delay, text, effectColor, duration,
					markPosition));
			}
			else
			{
				positionEffect.Initialize(text, effectColor, duration, markPosition);
			}
			return positionEffect;
		}

		public IEnumerator InitializePositionEffectWithDelay(PositionEffect positionEffect, float delay, string text, Color effectColor, float duration, bool markPosition)
		{
			positionEffect.Initialize("", effectColor, 10f, markPosition);

			yield return new WaitForSeconds(delay);

			positionEffect.Initialize(text, effectColor, duration, markPosition);
		}

		public PositionEffect ShowStablePositionEffect(Position position, string text, Color? color = null, bool markPosition = false)
		{
			PositionEffect positionEffect = FetchPositionEffectAtPosition(position);

			Color effectColor = color ?? Color.white;
			float duration = 100000f;

			positionEffect.Initialize(text, effectColor, duration, markPosition);
			return positionEffect;
		}

	    private PositionEffect FetchPositionEffectAtPosition(Position position)
		{
			if (_positionEffectPrefab == null)
			{
				_positionEffectPrefab = Resources.Load<PositionEffect>("Prefabs/UI/" + nameof(PositionEffect));
			}
			Vector3 position3 = _unityGridInfoProvider.GetCellCenterWorld(position);
			GameObject positionEffectObject = PoolingManager.Fetch(PoolingManager.PositionEffect, position3, Quaternion.identity);
			PositionEffect positionEffect = positionEffectObject.GetComponent<PositionEffect>();
			return positionEffect;
		}
	}
}