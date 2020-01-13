using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Piec.Tests.Pathfinding
{
	[TestFixture]
	public class NaturalLineCalculatorTests
	{
		[Test]
		public void GetNaturalJumpPoints_OneJumpPoint_ReturnsCorrectResult()
		{
			var jumpPoints = new[] {new Position(0, 0)};
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(jumpPoints);
		}

		[Test]
		public void GetNaturalJumpPoints_TwoJumpPoints_ReturnsCorrectResult()
		{
			var jumpPoints = new[] { new Position(0, 0), new Position(1, 0) };
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(jumpPoints);
		}

		[Test]
		public void GetNaturalJumpPoints_ThreeJumpPointsInLine_ReturnsCorrectResultWithFirstAndLast()
		{
			var jumpPoints = new[] { new Position(0, 0), new Position(1, 0), new Position(2, 0) };
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(new[] { new Position(0, 0), new Position(2, 0) }, options => options.WithStrictOrdering());
		}

		[Test]
		public void GetNaturalJumpPoints_FiveJumpPointsInLine_ReturnsCorrectResultWithFirstAndLast()
		{
			var jumpPoints = new[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 0), new Position(4, 0) };
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(new[] { new Position(0, 0), new Position(4, 0) }, options => options.WithStrictOrdering());
		}

		[Test]
		public void GetNaturalJumpPoints_ThreeJumpPointsInLineAndThenOneDiagonal_ReturnsCorrectResultWithFirstAndSecondLastAndLast()
		{
			var jumpPoints = new[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 1)};
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(new[] { new Position(0, 0), new Position(2, 0), new Position(3, 1) }, 
				options => options.WithStrictOrdering());
		}

		[Test]
		public void GetNaturalJumpPoints_MultipleJumpPoints_ReturnsCorrectResult()
		{
			// input:
			//......*
			//...***.
			//***....

			// expected:
			//......*
			//...*.*.
			//*.*....
			
			var jumpPoints = new[]
			{
	new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 1), new Position(4, 1),new Position(5, 1),new Position(6, 2),
			};
			var calculator = new NaturalLineCalculator(It.IsAny<IRasterLineCreator>());

			IList<Position> result = calculator.GetNaturalJumpPoints(jumpPoints);

			result.Should().BeEquivalentTo(
	new[] { new Position(0, 0), new Position(2, 0), new Position(3, 1), new Position(5, 1), new Position(6, 2) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void GetFirstLongestNaturalLine_ReturnsCorrectResult()
		{
			// For:
			// ..2****3
			// .*..####
			// 1....... (where 1 is (0,0), 2 is (2,2) and 3 is (7,2))
			// 
			// Should give:
			// ....**..
			// ..**####
			// **......
			var calculator = new NaturalLineCalculator(new BresenhamLineCreator());

			Func<Position, bool> isWalkable = position =>
			{
				if (position == new Position(4, 1) || position == new Position(5, 1)
				    || position == new Position(6, 1) || position == new Position(7, 1))
					return false;
				return true;
			};

			IList<Position> result 
				= calculator.GetFirstLongestNaturalLine(new[] {new Position(0, 0), new Position(2, 2), new Position(7, 2)}, isWalkable);

			result.Should().BeEquivalentTo(new[]
			{
				new Position(0,0),new Position(1,0),new Position(2,1),new Position(3,1),new Position(4,2),new Position(5,2),
			}, options => options.WithStrictOrdering());
		}
	}
}