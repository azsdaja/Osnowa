namespace Tests.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using FluentAssertions;
	using Moq;
	using NUnit.Framework;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Pathfinding;

	[TestFixture]
	public class PathfinderTests
	{
		[TestCase(-3,-3)]
		[TestCase(0,0)]
		[TestCase(3,3)]
		public void FindJumpPointsWithJps_IntegrationTest_AllGridIsWalkable_ReturnsCorrectPath(int xOffset, int yOffset)
		{
			/* Illustration:
			  ........
			  ...jt...
			  ..sj....
			  ........*/

			Position minPositionInUnityGrid = new Position(-10, -10);
			int unityGridXSize = 20;
			int unityGridYSize = 20;
			Position offset = new Position(xOffset, yOffset);
			var startPosition = new Position(0,0) + offset;
			var targetPosition = new Position(2, 1) + offset;
			var expectedMiddleJumpPoint = new Position(1, 1) + offset;
			var expectedAlternativeMiddleJumpPoint = new Position(1, 0) + offset;
			PathfindingDataHolder pathfindingData = new PathfindingDataHolder(unityGridXSize, unityGridYSize);
			for (int x = 0; x < unityGridXSize; x++)
				for (int y = 0; y < unityGridYSize; y++)
				{
					var position = new Position(x, y);
					pathfindingData.UpdateWalkability(position, true);
				}
			var contextManager = Mock.Of<IOsnowaContextManager>(m => m.Current.PathfindingData == pathfindingData);
			
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
			  ........
			  ...#....
			  ..s#t...
			  ...j....*/

			Position minPositionInUnityGrid = new Position(-10, -10);
			int unityGridXSize = 20;
			int unityGridYSize = 20;
			var startPosition = new Position(0,0);
			var targetPosition = new Position(2,0);
			var expectedMiddleJumpPoint = new Position(1,-1);
			Func<Position, bool> isWalkable = position =>
			{
				if (position == new Position(1, 0) || position == new Position(1, 1))
					return false;
				return true;
			};
			var grid = Mock.Of<IGrid>(
				f => f.IsWalkable(It.Is<Position>(v => isWalkable(v))) == true
				&& f.IsWalkable(It.Is<Position>(v => !isWalkable(v))) == false
				&& f. MinPosition == minPositionInUnityGrid
				&& f.XSize == unityGridXSize
				&& f.YSize == unityGridYSize);
			IRasterLineCreator bresenham = new BresenhamLineCreator();

			var pathfinder = new Pathfinder(default, new NaturalLineCalculator(bresenham), bresenham);

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
		public void GetJumpPoints_IntegrationTest_ThereIsNoPathToTarget_ReturnsNull()
		{
			/* Illustration:
			  ........
			  .###....
			  .#s#t...
			  .###....*/

			Position minPositionInUnityGrid = new Position(-10, -10);
			int unityGridXSize = 20;
			int unityGridYSize = 20;
			var startPosition = new Position(0,0);
			var targetPosition = new Position(2,0);
			Func<Position, bool> isWalkable = position =>
			{
				if ( // surrounding start point with walls
					position == new Position(-1, 1) || position == new Position(0, 1) || position == new Position(1, 1)
					|| position == new Position(-1, 0) || position == new Position(1, 0)
					|| position == new Position(-1, -1) || position == new Position(0, -1) || position == new Position(1, -1))
					return false;
				return true;
			};
			var grid = Mock.Of<IGrid>(
				f => f.IsWalkable(It.Is<Position>(v => isWalkable(v))) == true
				&& f.IsWalkable(It.Is<Position>(v => !isWalkable(v))) == false
				&& f. MinPosition == minPositionInUnityGrid
				&& f.XSize == unityGridXSize
				&& f.YSize == unityGridYSize);
			IRasterLineCreator bresenham = new BresenhamLineCreator();

			var pathfinder = new Pathfinder(default, new NaturalLineCalculator(bresenham), bresenham);

			IList<Position> jumpPoints = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition).Positions;
			IList<Position> jumpPointsFromSpatialAstar = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition).Positions;

			jumpPoints.Should().BeNull();
			jumpPointsFromSpatialAstar.Should().BeNull();
		}

		[Test]
		public void GetJumpPoints_TargetNotInBounds_ReturnsNull()
		{
			/* Illustration:
			  
			  ...... t
			  s.....

			*/

			Position minPositionInUnityGrid = new Position(0, 0);
			int unityGridXSize = 6;
			int unityGridYSize = 2;
			var startPosition = new Position(0,0);
			var targetPosition = new Position(7,1);
			Func<Position, bool> isWalkable = position =>
			{
				return true;
			};
			var grid = Mock.Of<IGrid>(
				f => f.IsWalkable(It.Is<Position>(v => isWalkable(v))) == true
				&& f.IsWalkable(It.Is<Position>(v => !isWalkable(v))) == false
				&& f. MinPosition == minPositionInUnityGrid
				&& f.XSize == unityGridXSize
				&& f.YSize == unityGridYSize);
			IRasterLineCreator bresenham = new BresenhamLineCreator();
			var pathfinder = new Pathfinder(default, new NaturalLineCalculator(bresenham), bresenham);

			IList<Position> jumpPoints = pathfinder.FindJumpPointsWithJps(startPosition, targetPosition).Positions;
			IList<Position> jumpPointsFromSpatialAstar = pathfinder.FindJumpPointsWithSpatialAstar(startPosition, targetPosition).Positions;

			jumpPoints.Should().BeNull();
			jumpPointsFromSpatialAstar.Should().BeNull();
		}
	}
}