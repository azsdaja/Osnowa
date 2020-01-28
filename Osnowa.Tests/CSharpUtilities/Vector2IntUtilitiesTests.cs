namespace Osnowa.Tests.CSharpUtilities
{
	using FluentAssertions;
	using NUnit.Framework;

	[TestFixture]
	public class PositionUtilitiesTests
	{
		[TestCase(0,0, 0,0)]
		[TestCase(0,1, 0,0)]
		[TestCase(1,0, 1,0)]
		[TestCase(1,1, 1,0)]
		[TestCase(5,1, 1,0)]
		[TestCase(-1,0, -1,0)]
		[TestCase(-3,-5, -1,0)]
		public void SnapToXAxisNormalized_ReturnsCorrectResult(int x, int y, int expectedX, int expectedY)
		{
			Position result = PositionUtilities.SnapToXAxisNormalized(new Position(x, y));

			result.x.Should().Be(expectedX);
			result.y.Should().Be(expectedY);
		}	

		[TestCase(0,0, 0,0)]
		[TestCase(0,1, 0,1)]
		[TestCase(1,0, 0,0)]
		[TestCase(1,1, 0,1)]
		[TestCase(5,1, 0,1)]
		[TestCase(-1,-1, 0,-1)]
		[TestCase(-3,-5, 0,-1)]
		public void SnapToYAxisNormalized_ReturnsCorrectResult(int x, int y, int expectedX, int expectedY)
		{
			Position result = PositionUtilities.SnapToYAxisNormalized(new Position(x, y));

			result.x.Should().Be(expectedX);
			result.y.Should().Be(expectedY);
		}

		[Test]
		public void Average_CalculatesCorrectAverage()
		{
			Position result = PositionUtilities.Average(new[] {new Position(-5, 0), new Position(0, 3), new Position(20, 0)});

			result.Should().Be(new Position(5, 1));
		}

		[TestCase(0,0, 0,0, 0)]
		[TestCase(0,0, 1,0, 1)]
		[TestCase(0,0, 1,0, 1)]
		[TestCase(0,0, 2,0, 2)]
		[TestCase(0,0, 1,1, 1)]
		[TestCase(0,0, 3,3, 3)]
		[TestCase(0,0, 2,1, 2)]
		[TestCase(0,0, 5,3, 5)]
		[TestCase(-5,-5, -3,-3, 2)]
		public void WalkDistance_ReturnsCorrectWalkDistance(int x1, int y1, int x2, int y2, int expectedDistance)
		{
			var start = new Position(x1, y1);
			var target = new Position(x2, y2);

			int result = PositionUtilities.WalkDistance(start, target);

			result.Should().Be(expectedDistance);
		}

		private static readonly object[] GetFittingPositionsTestCases =
		{
			new object[] {new Position(0, 0), new Position(1, 0), default(Position?) },
			new object[] {new Position(0, 0), new Position(0, 1), default(Position?) },
			new object[] {new Position(1, 0), new Position(0, 0), default(Position?) },
			new object[] {new Position(0, 1), new Position(0, 0), default(Position?) },
			new object[] {new Position(0, 0), new Position(2, 2), default(Position?) },
			new object[] {new Position(2, 2), new Position(0,0), default(Position?) },
			new object[] {new Position(1, 1), new Position(0,0), new Position(0, 1),  },
			new object[] {new Position(-1, -1), new Position(0,0), new Position(0, -1),  },
			new object[] {new Position(1, -1), new Position(0,0), new Position(1, 0),  },
			new object[] {new Position(-1, 1), new Position(0,0), new Position(-1, 0),  },
			new object[] {new Position(5, 5), new Position(0,0), new Position(0, 1),  },
			new object[] {new Position(-5, -5), new Position(0,0), new Position(0, -1),  },
			new object[] {new Position(5, -5), new Position(0,0), new Position(1, 0),  },
			new object[] {new Position(-5, 5), new Position(0,0), new Position(-1, 0),  },
		};

		[TestCaseSource(nameof(GetFittingPositionsTestCases))]
		public void GetFittingPosition_ReturnsCorrectResult(Position current, Position previous, Position? expectedResult)
		{
			Position? result = PositionUtilities.GetFittingPosition(current, previous);

			result.Should().Be(expectedResult);
		}
	}
}