namespace Tests.Pathfinding
{
	using System.Collections.Generic;
	using FluentAssertions;
	using NUnit.Framework;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Pathfinding;

	[TestFixture]
	public class PathfinderTests
	{
		// legend: s - start, t - target, j - jump point 
		
		[Test]
		public void FindJumpPointsWithJps_IntegrationTest_AllGridIsWalkable_ReturnsCorrectPath()
		{
			/* Illustration:
			  ......
			  .jt... - either of the j's should be used
			  sj....
			  ......*/

			int unityGridXSize = 20;
			int unityGridYSize = 20;
			var startPosition = new Position(0,1);
			var targetPosition = new Position(2, 2);
			var expectedMiddleJumpPoint = new Position(1, 1);
			var expectedAlternativeMiddleJumpPoint = new Position(1, 2);
			var contextManager = CreateContextManager(unityGridXSize, unityGridYSize);
			
			IRasterLineCreator bresenham = new BresenhamLineCreator();
			var pathfinder = new Pathfinder(contextManager, new NaturalLineCalculator(bresenham), bresenham);

			IList<Position> jumpPoints = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition).Positions;
			IList<Position> jumpPointsFromSpatialAstar = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition).Positions;

			jumpPoints.Count.Should().Be(3);
			jumpPoints[0].Should().Be(startPosition);
			(jumpPoints[1] == expectedMiddleJumpPoint || jumpPoints[1] == expectedAlternativeMiddleJumpPoint).Should().BeTrue();
			jumpPoints[2].Should().Be(targetPosition);
			
			jumpPointsFromSpatialAstar.Count.Should().Be(3);
			jumpPointsFromSpatialAstar[0].Should().Be(startPosition);
			(jumpPointsFromSpatialAstar[1] == expectedMiddleJumpPoint ||
			 jumpPointsFromSpatialAstar[1] == expectedAlternativeMiddleJumpPoint).Should().BeTrue();
			jumpPointsFromSpatialAstar[2].Should().Be(targetPosition);
		}

		[Test]
		public void GetJumpPoints_IntegrationTest_WallIsBlockingWayToTarget_ReturnsCorrectPath()
		{
			/* Illustration:
			  ......
			  .#....
			  s#t...
			  .j....*/

			int unityGridXSize = 20;
			int unityGridYSize = 20;
			var startPosition = new Position(0,1);
			var targetPosition = new Position(2,1);
			var expectedMiddleJumpPoint = new Position(1,0);
			IRasterLineCreator bresenham = new BresenhamLineCreator();
			
			IOsnowaContextManager contextManager = CreateContextManager(unityGridXSize, unityGridYSize);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(1, 1), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(1, 2), false);
			var pathfinder = new Pathfinder(contextManager, new NaturalLineCalculator(bresenham), bresenham);

			IList<Position> jumpPoints = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition).Positions;
			IList<Position> jumpPointsFromSpatialAstar = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition).Positions;

			jumpPoints.Count.Should().Be(3);
			jumpPoints[0].Should().Be(startPosition);
			jumpPoints[1].Should().Be(expectedMiddleJumpPoint);
			jumpPoints[2].Should().Be(targetPosition);
			
			jumpPointsFromSpatialAstar.Count.Should().Be(3);
			jumpPointsFromSpatialAstar[0].Should().Be(startPosition);
			jumpPointsFromSpatialAstar[1].Should().Be(expectedMiddleJumpPoint);
			jumpPointsFromSpatialAstar[2].Should().Be(targetPosition);
		}

		[Test]
		public void GetJumpPoints_IntegrationTest_ThereIsNoPathToTarget_ReturnsTargetUnreachable()
		{
			/* Illustration:
			  .......
			  ###....
			  #s#t...
			  ###....*/

			int unityGridXSize = 20;
			int unityGridYSize = 20;
			var startPosition = new Position(1,1);
			var targetPosition = new Position(3,1);
			IRasterLineCreator bresenham = new BresenhamLineCreator();
			IOsnowaContextManager contextManager = CreateContextManager(unityGridXSize, unityGridYSize);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(0, 0), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(0, 1), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(0, 2), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(1, 0), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(1, 2), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(2, 0), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(2, 1), false);
			contextManager.Current.PathfindingData.UpdateWalkability(new Position(2, 2), false);
			var pathfinder = new Pathfinder(contextManager, new NaturalLineCalculator(bresenham), bresenham);

			PathfindingResponse jpsResult = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition);
			jpsResult.Result.Should().Be(PathfindingResult.FailureTargetUnreachable);
			jpsResult.Positions.Should().BeNull();
			
			PathfindingResponse aStarResult = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition);
			aStarResult.Result.Should().Be(PathfindingResult.FailureTargetUnreachable);
			aStarResult.Positions.Should().BeNull();
		}

		[Test]
		public void GetJumpPoints_TargetNotInBounds_ReturnsTargetUnreachable()
		{
			/* Illustration:
			  
			  ...... t
			  s.....

			*/

			int unityGridXSize = 6;
			int unityGridYSize = 2;
			var startPosition = new Position(0,0);
			var targetPosition = new Position(7,1);
			IRasterLineCreator bresenham = new BresenhamLineCreator();
			var pathfinder = new Pathfinder(CreateContextManager(unityGridXSize, unityGridYSize), new NaturalLineCalculator(bresenham), bresenham);

			PathfindingResponse jpsResult = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition);
			jpsResult.Result.Should().Be(PathfindingResult.FailureTargetUnreachable);
			jpsResult.Positions.Should().BeNull();
			
			PathfindingResponse aStarResult = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition);
			aStarResult.Result.Should().Be(PathfindingResult.FailureTargetUnreachable);
			aStarResult.Positions.Should().BeNull();
		}

		private IOsnowaContextManager CreateContextManager(int gridXSize, int gridYSize)
		{
			var manager = new OsnowaContextManager();
			manager.ReplaceContext(new OsnowaContext(gridXSize, gridYSize));
			return manager;
		}
	}
}