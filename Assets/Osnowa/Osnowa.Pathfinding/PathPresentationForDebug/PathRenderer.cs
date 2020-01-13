namespace Osnowa.Osnowa.Pathfinding.PathPresentationForDebug
{
	using System.Collections.Generic;
	using System.Linq;
	using Core;
	using global::Osnowa.Osnowa.Grid;
	using global::Osnowa.Osnowa.Unity;
	using UnityEngine;
	using UnityUtilities;

	public class PathRenderer : IPathRenderer
	{
		private readonly Material _lineMaterial;
		private readonly IUnityGridInfoProvider _unityGridInfoProvider;
		private readonly IGrid _grid;
		private readonly INaturalLineCalculator _naturalLineCalculator;

		public PathRenderer(Material lineMaterial, IUnityGridInfoProvider unityGridInfoProvider, INaturalLineCalculator naturalLineCalculator, IGrid grid)
		{
			_lineMaterial = lineMaterial;
			_unityGridInfoProvider = unityGridInfoProvider;
			_naturalLineCalculator = naturalLineCalculator;
		    _grid = grid;

		}

		public void ShowPath(IList<Position> pathPoints, float score)
		{
			if (!pathPoints.Any()) return;
			var line = new GameObject("PathFragment");
			var lineRenderer = line.AddComponent<LineRenderer>();
			SetupLineRenderer(pathPoints, lineRenderer);
			int middlePathPointIndex = pathPoints.Count / 2;
			Position labelPositionInGrid = pathPoints[middlePathPointIndex];
			Vector3 labelPosition = _unityGridInfoProvider.GetCellCenterWorld(labelPositionInGrid);
			line.transform.position = labelPosition;
			var label = line.AddComponent<Label>();
			label.Text = score.ToString();

			foreach (var Position in pathPoints)
			{
				var node = new GameObject("MyPathNode");
				SpriteRenderer nodeSprite = node.AddComponent<SpriteRenderer>();
				nodeSprite.sortingLayerName = "HUD";
				nodeSprite.transform.position = _unityGridInfoProvider.GetCellCenterWorld(Position);
				nodeSprite.sprite = Resources.Load<Sprite>(@"Sprites\Characters\Misc_tiles\19");
				Object.Destroy(node.gameObject, 10.0f);
			}

			Object.Destroy(line.gameObject, 8.0f);
		}

		public void ShowNaturalWay(List<Position> jumpPoints, float score)
		{
			IList<Position> naturalWay = _naturalLineCalculator.GetFirstLongestNaturalLine(jumpPoints, _grid.IsWalkable);

			var line = new GameObject("PathFragment");
			var lineRenderer = line.AddComponent<LineRenderer>();
			SetupLineRenderer(naturalWay, lineRenderer);
			lineRenderer.startColor = Color.blue;
			lineRenderer.endColor = Color.blue;

			Object.Destroy(line.gameObject, 2.5f);
		}

		private void SetupLineRenderer(IList<Position> pathPoints, LineRenderer lineRenderer)
		{
			lineRenderer.startColor = Color.green;
			lineRenderer.endColor = Color.green;
			lineRenderer.material = _lineMaterial;
			lineRenderer.sortingLayerName = "HUD";
			IList<Vector3> jumpPointsInWorld = pathPoints.Select(p => _unityGridInfoProvider.GetCellCenterWorld(p)).ToList();
			lineRenderer.startWidth = .05f;
			lineRenderer.endWidth = .05f;
			lineRenderer.positionCount = jumpPointsInWorld.Count;
			lineRenderer.SetPositions(jumpPointsInWorld.ToArray());
		}
	}
}
