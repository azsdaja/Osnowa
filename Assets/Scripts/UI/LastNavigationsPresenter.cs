namespace UI
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using UnityEngine;
	using UnityUtilities;

	public class LastNavigationsPresenter : MonoBehaviour
	{
		public Grid Grid;

		public int PathsKept = 10;

		public Queue<IList<Position>> LastStraightPaths = new Queue<IList<Position>>();
		public Queue<IList<Position>> LastNotStraightPaths = new Queue<IList<Position>>();

		public void AddNewStraightPath(IList<Position> path)
		{
			LastStraightPaths.Enqueue(path);
			if (LastStraightPaths.Count > PathsKept)
			{
				LastStraightPaths.Dequeue();
			}
		}

		public void AddNewUnstraightPath(IList<Position> path)
		{
			LastNotStraightPaths.Enqueue(path);
			if (LastNotStraightPaths.Count > PathsKept)
			{
				LastNotStraightPaths.Dequeue();
			}
		}

		// Update is called once per frame
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.magenta;
			foreach (IList<Position> path in LastStraightPaths)
			{
				DrawPath(path);
			}
		
			Gizmos.color = Color.green;
			foreach (IList<Position> path in LastNotStraightPaths)
			{
				DrawPath(path);
			}
		}

		private void DrawPath(IList<Position> path)
		{
			for (int i = 1; i < path.Count; i++)
			{
				Position previous = path[i - 1];
				Position current = path[i];
				Vector3 previousWorldPosition = Grid.GetCellCenterWorld(previous.ToVector3Int());
				Vector3 currentWorldPosition = Grid.GetCellCenterWorld(current.ToVector3Int());
				Gizmos.DrawLine(previousWorldPosition, currentWorldPosition);
			}
		}
	}
}
