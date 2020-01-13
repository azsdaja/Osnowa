using static UnityEngine.Object;

namespace GameLogic.AI.Navigation
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using UI;

	public class DebugPathPresenter : IDebugPathPresenter
	{
		public void PresentStraight(IList<Position> path)
		{
			#if UNITY_EDITOR
			LastNavigationsPresenter presenterGameObject = FindObjectOfType<LastNavigationsPresenter>();
			presenterGameObject.AddNewStraightPath(path);
			#endif
		}

		public void PresentNonStraight(List<Position> path)
		{
			#if UNITY_EDITOR
			LastNavigationsPresenter presenterGameObject = FindObjectOfType<LastNavigationsPresenter>();
			presenterGameObject.AddNewUnstraightPath(path);
			#endif
		}
	}
}