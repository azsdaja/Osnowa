namespace Osnowa.Osnowa.Unity
{
	using UnityEngine;
	using UnityEngine.Events;

	public class OnStartEventEmitter : MonoBehaviour
	{
		public UnityEvent OnStart;

		void Start()
		{
			OnStart.Invoke();
		}
	}
}
