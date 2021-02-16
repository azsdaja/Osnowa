namespace Osnowa.Osnowa.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Context;
	using Core;
	using Fov;
	using Libraries.PathPlannersLib;
	using Libraries.SpatialAStar.SpatialAStar.Algorithm;

	/// <inheritdoc/>
	public partial class Pathfinder : IPathfinder
	{
		private readonly IOsnowaContextManager _contextManager;
		private readonly INaturalLineCalculator _naturalLineCalculator;
		private readonly IRasterLineCreator _rasterLineCreator;
		private bool _ready;
		
		private Jps _jps;
		private JpsTightDiagonal _jpsTightDiagonal;
		private JpsStrictWalk _jpsStrictWalk;
		private SpatialAStar<MyPathNode, Position> _spatialAStar;
		
		public Pathfinder(IOsnowaContextManager contextManager, INaturalLineCalculator naturalLineCalculator, IRasterLineCreator rasterLineCreator)
		{
			_contextManager = contextManager;
			_naturalLineCalculator = naturalLineCalculator;
			_rasterLineCreator = rasterLineCreator;

			_contextManager.ContextReplaced += InitializeAlgorithms;
		}

		/// <summary>
		/// Should be used only in object trees that are not created by dependency injection (e.g. map generation).
		/// </summary>
		public static Pathfinder Create(IOsnowaContextManager contextManager)
		{
			var bresenhamLineCreator = new BresenhamLineCreator();
			
			//Grid gridForMapGeneration = new Grid(contextManager, pathfindingDataHolder, true);
			var pathfinder = new Pathfinder(contextManager, new NaturalLineCalculator(bresenhamLineCreator), bresenhamLineCreator);
			//pathfinder.InitializeAlgorithms();
			return pathfinder;
		}

		public void InitializeAlgorithms(IOsnowaContext newContext)
		{
			_jps = new Jps(newContext.PathfindingData.WallMatrixForJps);
			_jpsTightDiagonal = new JpsTightDiagonal(newContext.PathfindingData.WallMatrixForJps);
			_jpsStrictWalk = new JpsStrictWalk(newContext.PathfindingData.WallMatrixForJps);
			
			_spatialAStar = new SpatialAStar<MyPathNode, Position>(newContext.PathfindingData.PathNodeMatrixForSpatialAStar);

			_ready = true;
		}
		
		/// <inheritdoc/>
		public PathfindingResponse FindJumpPointsWithJps(Position startPosition, Position targetPosition, JpsMode mode = JpsMode.AllowDiagonalBetweenWalls)
		{
			if (!_ready) InitializeAlgorithms(_contextManager.Current);
			
			Point startPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(startPosition);
			Point targetPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(targetPosition);
			bool[,] wallMatrix = _contextManager.Current.PathfindingData.WallMatrixForJps; 
			if (targetPoint.X >= wallMatrix.GetLength(0) || targetPoint.Y >= wallMatrix.GetLength(1) || wallMatrix[targetPoint.X, targetPoint.Y])
			{
				return new PathfindingResponse(PathfindingResult.FailureTargetUnreachable);
			}

			List<Point> path;
			int nodesOpen, nodesClosed;
			if (mode == JpsMode.AllowDiagonalBetweenWalls)
				path = _jpsTightDiagonal.FindPath(startPoint, targetPoint, out nodesOpen, out nodesClosed);
			else if (mode == JpsMode.Normal)
				path = _jps.FindPath(startPoint, targetPoint, out nodesOpen, out nodesClosed);
			else
				path = _jpsStrictWalk.FindPath(startPoint, targetPoint, out nodesOpen, out nodesClosed);

			//Debug.Log($"{(path.Any() ? "" : "Failed ")} Path from {startPoint} to {targetPoint} with {path.Count} steps, {nodesOpen} open nodes, {nodesClosed} closed nodes.");
			if (!path.Any())
			{
				return new PathfindingResponse(PathfindingResult.FailureTargetUnreachable);
			}

			List<Position> resultJumpPointsInGrid = path.Select(_contextManager.Current.PathfindingData.PointToNonZeroBasedPosition).ToList();

			List<Position> naturalJumpPoints = _naturalLineCalculator.GetNaturalJumpPoints(resultJumpPointsInGrid).ToList();
			naturalJumpPoints.Reverse();
			return new PathfindingResponse(naturalJumpPoints);
		}

		/// <inheritdoc/>
		public PathfindingResponse FindJumpPointsWithSpatialAstar(Position startPoint, Position targetPoint)
		{
			if (!_ready) InitializeAlgorithms(_contextManager.Current);

			Point startPointPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(startPoint);
			Point targetPointPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(targetPoint);

			int nodesOpen, nodesClosed;
			LinkedList<MyPathNode> path = _spatialAStar.Search(startPointPoint, targetPointPoint, Position.Zero, out nodesOpen, out nodesClosed);

			//Debug.Log($"{(path==null?"Failed " : "")} Path from {startPoint} to {targetPoint} with {path?.Count} steps, {nodesOpen} open nodes, {nodesClosed} closed nodes.");

			if (path == null || !path.Any())
			{
				return new PathfindingResponse(PathfindingResult.FailureTargetUnreachable);
			}

			List<Position> resultJumpPointsInGrid = path.Select(p => p.Position).ToList();

			List<Position> naturalJumpPoints = _naturalLineCalculator.GetNaturalJumpPoints(resultJumpPointsInGrid).ToList();
			return new PathfindingResponse(naturalJumpPoints);
		}

		public PathfindingResponse FindFullPathWithSpatialAstar(Position startPosition, Position targetPosition)
		{
			if (!_ready) InitializeAlgorithms(_contextManager.Current);

			Point startPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(startPosition);
			Point targetPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(targetPosition);

			int nodesOpen, nodesClosed;
			LinkedList<MyPathNode> path = _spatialAStar.Search(startPoint, targetPoint, Position.Zero, out nodesOpen, out nodesClosed);

			//Debug.Log($"{(path == null ? "Failed " : "")} Path from {startPoint} to {targetPoint} with {path?.Count} steps, {nodesOpen} open nodes, {nodesClosed} closed nodes.");

			if (path == null || !path.Any())
			{
				return new PathfindingResponse(PathfindingResult.FailureTargetUnreachable);
			}

			List<Position> resultPositionsInGrid = path.Select(p => p.Position).ToList();
			return new PathfindingResponse(resultPositionsInGrid);
		}

		public List<Position> DebugFindFullPathWithSpatialAstar(Position startPoint, Position targetPoint, int[,] pathfindingDebug)
		{
			if (!_ready) InitializeAlgorithms(_contextManager.Current);

			Point startPointPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(startPoint);
			Point targetPointPoint = _contextManager.Current.PathfindingData.PositionToZeroBasedPoint(targetPoint);

			LinkedList<MyPathNode> path = _spatialAStar.DebugSearch(startPointPoint, targetPointPoint, Position.Zero, pathfindingDebug);

			if (path == null || !path.Any())
			{
				return null;
			}

			List<Position> resultPositionsInGrid = path.Select(p => p.Position).ToList();
			return resultPositionsInGrid;
		}

		public bool LineIsWalkable(Position startPoint, Position targetPoint, Func<Position, bool> isWalkable)
		{
			if (!_ready) InitializeAlgorithms(_contextManager.Current);

			foreach (Position position in _rasterLineCreator.GetRasterLine(startPoint.x, startPoint.y, targetPoint.x, targetPoint.y))
			{
				if (!isWalkable(position))
					return false;
			}
			return true;
		}
	}
}