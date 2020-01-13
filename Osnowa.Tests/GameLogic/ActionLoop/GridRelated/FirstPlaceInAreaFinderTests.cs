using System.Linq;
using Assets.Osnowa.Osnowa.Core;
using Assets.Osnowa.Osnowa.RNG;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Osnowa.Osnowa.Grid;
using Osnowa.Osnowa.Unity.GameLogic.GridRelated;
using Osnowa.Osnowa.Unity.UnityUtilities;

namespace Osnowa.Tests.GameLogic.ActionLoop.GridRelated
{
	[TestFixture]
	public class FirstPlaceInAreaFinderTests
	{
		[Test]
		public void FindForItem_FirstDestinationIsWalkableButOccupied_ReturnsFailure()
		{
			var checkedPosition = new Position(1, 1);
			IGrid gip = Mock.Of<IGrid>(p => p.IsWalkable(checkedPosition) == true);
			IEntityDetector entityDetector = Mock.Of<IEntityDetector>(d => 
															d.DetectEntities(checkedPosition) == new[]{new GameEntity()});
			var finder = new FirstPlaceInAreaFinder(gip, entityDetector, new RandomNumberGenerator(93432));

			Position? result = finder.FindForItem(checkedPosition);

			result.Should().BeNull();
		}

		[Test]
		public void FindForItem_NoNeighboursAndFirstDestinationIsUnoccupiedButWalkable_ReturnsFailure()
		{
			var checkedPosition = new Position(1, 1);
			IGrid gip = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == false);
			IEntityDetector entityDetector = Mock.Of<IEntityDetector>(d =>
															d.DetectEntities(checkedPosition) == Enumerable.Empty<GameEntity>());
			var finder = new FirstPlaceInAreaFinder(gip, entityDetector, new RandomNumberGenerator(93432));

			Position? result = finder.FindForItem(checkedPosition);

			result.Should().BeNull();
		}

		[Test]
		public void FindForItem_FirstDestinationIsWalkableAndUnoccupied_ReturnsFirstDestination()
		{
			var checkedPosition = new Position(1, 1);
			IGrid gip = Mock.Of<IGrid>(p => p.IsWalkable(checkedPosition) == true);
			IEntityDetector entityDetector = Mock.Of<IEntityDetector>(d =>
															d.DetectEntities(checkedPosition) == Enumerable.Empty<GameEntity>());
			var finder = new FirstPlaceInAreaFinder(gip, entityDetector, new RandomNumberGenerator(93432));

			Position? result = finder.FindForItem(checkedPosition);

			result.Should().Be(checkedPosition);
		}

		[Test]
		public void FindForItem_FirstDestinationAndMostNeighboursAreNotEligibleButOneNeighbourIs_ReturnsNeighbour()
		{
			var checkedPosition = new Position(1, 1);
			var eligibleNeighbour = new Position(2, 1);
			IGrid gip = Mock.Of<IGrid>(p =>
										p.IsWalkable(
											It.Is<Position>(v => PositionUtilities.Neighbours8(checkedPosition).Contains(v))
											) == false
											&& p.IsWalkable(eligibleNeighbour) == true);
			IEntityDetector entityDetector = Mock.Of<IEntityDetector>(d =>
															d.DetectEntities(checkedPosition) == Enumerable.Empty<GameEntity>());
			var finder = new FirstPlaceInAreaFinder(gip, entityDetector, new RandomNumberGenerator(93432));

			Position? result = finder.FindForItem(checkedPosition);

			result.Should().Be(eligibleNeighbour);
		}

		// todo: look below
		[Test, Ignore("reaching farther than to closest neighbours is not implemented yet")]
		public void FindForItem_OnePositionInFartherVicinityIsEligible_ReturnsThisPosition()
		{
			Assert.Fail();
		}
	}
}