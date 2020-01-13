namespace UnityUtilities
{
	using System;
	using System.Collections.Generic;
	using GGEZ;
	using Libraries.Cinemachine.Base.Runtime.Behaviours;
	using UnityEngine;

	public class CameraMouseControl : MonoBehaviour
	{
		private Vector3 _lastPosition;
		private const int CameraPriorityDelta = 2;

		public float MouseSensitivity = 0.008f;
		public int MaxZoomFactor = 4;

		public CinemachineVirtualCamera PannableCamera;
		public CinemachineVirtualCamera FollowPlayerCamera;
		private PerfectPixelCamera _pixelPerfectCamera;
		private IList<float> _approximateOrthographicSizesForPixelPerfectZoomFactors;
		private int _currentZoomFactor = 2;

		// Use this for initialization
		void Start()
		{
			_pixelPerfectCamera = GetComponent<PerfectPixelCamera>();
			_approximateOrthographicSizesForPixelPerfectZoomFactors = CreateApproximatedOrthographicSizesForPixelPerfectZoomFactors();

			PannableCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
			FollowPlayerCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
		}

		// Update is called once per frame

		void Update()
		{
			if (Input.GetMouseButtonDown(2) || (FollowPlayerCamera.Follow != null && Input.GetMouseButtonDown(0)))
			{
				_lastPosition = Input.mousePosition;

				if (PannableCamera.Priority < FollowPlayerCamera.Priority)
				{
					PannableCamera.Priority += CameraPriorityDelta;
					PannableCamera.transform.position = FollowPlayerCamera.transform.position;
				}
			}

			if (Input.GetMouseButton(2) || (FollowPlayerCamera.Follow != null && Input.GetMouseButton(0)))
			{
				var delta = Input.mousePosition - _lastPosition;
				float adjustedSensitivity = MouseSensitivity * Camera.main.orthographicSize;
				PannableCamera.transform.Translate(-delta.x * adjustedSensitivity, -delta.y * adjustedSensitivity, 0);
				_lastPosition = Input.mousePosition;
			}

			if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				//float currentOrthographicSize = 
				_currentZoomFactor -= 1;
				if (_currentZoomFactor == 0)
					_currentZoomFactor = 1;

				PannableCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
				FollowPlayerCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
			}

			if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.Plus) ||
															Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				_currentZoomFactor += 1;
				if (_currentZoomFactor > MaxZoomFactor)
					_currentZoomFactor = MaxZoomFactor;

				PannableCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
				FollowPlayerCamera.m_Lens.OrthographicSize = _approximateOrthographicSizesForPixelPerfectZoomFactors[_currentZoomFactor];
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				StopFollowingPlayer();
			}
		}

		private float[] CreateApproximatedOrthographicSizesForPixelPerfectZoomFactors()
		{
			int factorsCount = MaxZoomFactor;
			var result = new float[factorsCount + 1];
			result[0] = -1f;
			int currentDesiredZoomFactor = MaxZoomFactor;
			for (float orthoSize = 1f; orthoSize < 200f; orthoSize *= 1.2f)
			{
				int zoomFactor = Mathf.RoundToInt(_pixelPerfectCamera.CalculateZoomFactor(orthoSize));
				if (zoomFactor == currentDesiredZoomFactor)
				{
					result[zoomFactor] = orthoSize;
					currentDesiredZoomFactor -= 1;
					if (currentDesiredZoomFactor == 0)
					{
						return result;
					}
				}
			}

			throw new InvalidOperationException("Couldn't calculate all needed orthoSizes for zoom factors 1-5.");
		}

		public void StopFollowingPlayer()
		{
			if (PannableCamera.Priority > FollowPlayerCamera.Priority)
				PannableCamera.Priority -= CameraPriorityDelta;
		}
	}
}