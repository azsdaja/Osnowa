namespace Osnowa.Tests.FieldOfView
{
	using System.Collections.Generic;
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class FovSquareOutlineCreatorTests
	{
		[Test]
		public void CreateSquareOutline_RayLengthIsZero_ReturnsOnlyCenter()
		{
			var creator = new FovSquareOutlineCreator();
			var expectedOutlinePoints = new List<Position>{new Position(0,0)};

			IEnumerable<Position> outline = creator.CreateSquareOutline(fovCenter: new Position(0, 0), sightRange: 0);

			outline.Should().BeEquivalentTo(expectedOutlinePoints);
		}

		[Test]
		public void CreateSquareOutline_RayLengthIsOne_ReturnsAllNeighboursOfCenter()
		{
			var creator = new FovSquareOutlineCreator();
			var expectedOutlinePoints = new List<Position>
			{
				new Position(-1,-1),
				new Position(0,-1),
				new Position(1,-1),
				new Position(-1,0),
				new Position(1,0),
				new Position(-1,1),
				new Position(0,1),
				new Position(1,1),
			};

			IEnumerable<Position> outline = creator.CreateSquareOutline(fovCenter: new Position(0, 0), sightRange: 1);

			outline.Should().BeEquivalentTo(expectedOutlinePoints);
		}

		[TestCase(1,0)]
		[TestCase(0,1)]
		[TestCase(-15,24)]
		public void CreateSquareOutline_RayLengthIsOneAndCenterIsNotAtZero_ReturnsAllNeighboursOfCenter(int offsetX, int offsetY)
		{
			var offset = new Position(offsetX, offsetY);
			var creator = new FovSquareOutlineCreator();
			var expectedOutlinePoints = new List<Position>
			{
				new Position(-1,-1) + offset,
				new Position(0,-1) + offset,
				new Position(1,-1) + offset,
				new Position(-1,0) + offset,
				new Position(1,0) + offset,
				new Position(-1,1) + offset,
				new Position(0,1) + offset,
				new Position(1,1) + offset
			};

			IEnumerable<Position> outline = creator.CreateSquareOutline(fovCenter: offset, sightRange: 1);

			outline.Should().BeEquivalentTo(expectedOutlinePoints);
		}

		[Test]
		public void CreateSquareOutline_RayLengthIsTwo_ReturnsCorrectOutline()
		{
			var creator = new FovSquareOutlineCreator();
			var expectedOutlinePoints = new List<Position>
			{
				new Position(-2,-2),
				new Position(-1,-2),
				new Position(-0,-2),
				new Position(+1,-2),
				new Position(+2,-2),

				new Position(-2,-1),
				new Position(+2,-1),

				new Position(-2,0),
				new Position(+2,0),

				new Position(-2,+1),
				new Position(+2,+1),

				new Position(-2,+2),
				new Position(-1,+2),
				new Position(-0,+2),
				new Position(+1,+2),
				new Position(+2,+2),
			};

			IEnumerable<Position> outline = creator.CreateSquareOutline(fovCenter: new Position(0, 0), sightRange: 2);

			outline.Should().BeEquivalentTo(expectedOutlinePoints);
		}
	}
}