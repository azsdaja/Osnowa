namespace Tests.FieldOfView
{
	using System;
	using System.Collections.Generic;
	using FluentAssertions;
	using NUnit.Framework;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Fov;

	[TestFixture]
	public class BasicFovPostprocessorTests
	{
		[TestCase(0,0)]
		[TestCase(1,0)]
		[TestCase(0,1)]
		[TestCase(1,1)]
		[TestCase(-15,292)]
		public void GetBehindnessVectors_ReturnsCorrectVectors(int offsetX, int offsetY)
		{
			var postprocessor = new BasicFovPostprocessor();
			var offset = new Position(offsetX, offsetY);
			var currentPosition = new Position(1,1) + offset;
			var squareCenter = new Position(0,0) + offset;

			IEnumerable<Position> result = postprocessor.GetBehindnessVectors(currentPosition, squareCenter);

			result.Should().BeEquivalentTo(new List<Position>{new Position(1,0), new Position(0,1) });
		}

		[Test]
		public void GetBehindnessVectors_PositionHasNonZeroComponents_ReturnsCorrectVectors()
		{
			var postprocessor = new BasicFovPostprocessor();
			var positionPositivePositive = new Position(12,30);
			var positionNegativePositive = new Position(-5,14);
			var positionPositiveNegative = new Position(11,-1);
			var positionNegativeNegative = new Position(-5,-5);
			var squareCenter = new Position(0,0);

			IEnumerable<Position> resultPosPos = postprocessor.GetBehindnessVectors(positionPositivePositive, squareCenter);
			IEnumerable<Position> resultNegPos = postprocessor.GetBehindnessVectors(positionNegativePositive, squareCenter);
			IEnumerable<Position> resultPosNeg = postprocessor.GetBehindnessVectors(positionPositiveNegative, squareCenter);
			IEnumerable<Position> resultNegNeg = postprocessor.GetBehindnessVectors(positionNegativeNegative, squareCenter);

			resultPosPos.Should().BeEquivalentTo(new List<Position>{new Position(1,0), new Position(0,1) });
			resultNegPos.Should().BeEquivalentTo(new List<Position>{new Position(-1,0), new Position(0,1) });
			resultPosNeg.Should().BeEquivalentTo(new List<Position>{new Position(1,0), new Position(0,-1) });
			resultNegNeg.Should().BeEquivalentTo(new List<Position>{new Position(-1,0), new Position(0,-1) });
		}

		[Test]
		public void GetBehindnessVectors_PositionHasOneZeroComponent_ReturnsCorrectVectors()
		{
			var postprocessor = new BasicFovPostprocessor();
			var positionPositiveZero = new Position(12,0);
			var positionZeroPositive = new Position(0, 14);
			var positionNegativeZero = new Position(-7,0);
			var positionZeroNegative = new Position(0, -5);
			var squareCenter = new Position(0,0);

			IEnumerable<Position> resultPosZero = postprocessor.GetBehindnessVectors(positionPositiveZero, squareCenter);
			IEnumerable<Position> resultZeroPos = postprocessor.GetBehindnessVectors(positionZeroPositive, squareCenter);
			IEnumerable<Position> resultNegZero = postprocessor.GetBehindnessVectors(positionNegativeZero, squareCenter);
			IEnumerable<Position> resultZeroNeg = postprocessor.GetBehindnessVectors(positionZeroNegative, squareCenter);

			resultPosZero.Should().BeEquivalentTo(new List<Position>{new Position(1,0)  });
			resultZeroPos.Should().BeEquivalentTo(new List<Position>{new Position(0,1)  });
			resultNegZero.Should().BeEquivalentTo(new List<Position>{new Position(-1,0)  });
			resultZeroNeg.Should().BeEquivalentTo(new List<Position>{new Position(0,-1) });
		}

		[Test]
		public void PostprocessBasicFov_CorridorIsVisibleButSomeWallsNot_ReturnsWallsInVisibleSet()
		{
			/*
			 * Illustration (p = square center, i.e. viewer; dot = visible floor, W = visible wall, w = not visible wall):
			 * 
			 * WWwWWW
			 * p.....
			 * WWWwWW
			 * 
			 */

			var postprocessor = new BasicFovPostprocessor();
			Func<Position, bool> isWalkable = position => position.y == 0; // see picture, all tiles except those with y=0 are walls.
			var visibleBeforePostprocessing = new HashSet<Position>
			{
				new Position(0, 1), new Position(1, 1),                       new Position(3, 1), new Position(4 ,1), new Position(5 ,1), 
				new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 0), new Position(4 ,0), new Position(5 ,0), 
				new Position(0,-1), new Position(1, -1),new Position(2, -1),                      new Position(4 ,-1),new Position(5 ,-1), 
			};

			IEnumerable<Position> postprocessingResult = postprocessor.PostprocessBasicFov(visibleBeforePostprocessing, new Position(0, 0), 5, isWalkable);

			postprocessingResult.Should().BeEquivalentTo(new[]{new Position(2,1), new Position(3,-1) });
		}

		[Test]
		public void PostprocessBasicFov_CorridorIsVisibleButSomeWallsAreNot_WallsOutsideOfVisibilityRangeAreNotVisible()
		{
			/*
			 * Illustration (p = square center, i.e. viewer; dot = visible floor, W = visible wall, w = not visible wall):
			 * 
			 * WWwWWw
			 * p.....
			 * WWWWWw
			 * 
			 */

			var postprocessor = new BasicFovPostprocessor();
			Func<Position, bool> isWalkable = position => position.y == 0; // see picture, all tiles except those with y=0 are walls.
			var visibleBeforePostprocessing = new HashSet<Position>
			{
				new Position(0, 1), new Position(1, 1),                       new Position(3, 1), new Position(4 ,1), 
				new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 0), new Position(4 ,0), new Position(5 ,0),
				new Position(0,-1), new Position(1, -1),new Position(2, -1),new Position(3, -1),new Position(4 ,-1),
			};
			int rayLength = 5; // not enough to reach the walls that are most to the right

			IEnumerable<Position> postprocessingResult = 
				postprocessor.PostprocessBasicFov(visibleBeforePostprocessing, new Position(0, 0), rayLength, isWalkable);

			postprocessingResult.Should().NotContain(new Position(5,1));
			postprocessingResult.Should().NotContain(new Position(5,-1));
			postprocessingResult.Should().BeEquivalentTo(new[]{ new Position(2, 1)});
		}
	}
}