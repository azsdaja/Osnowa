namespace Tests.FieldOfView
{
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using NUnit.Framework;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.FOV;

	[TestFixture]
	public class BresenhamLineCreatorTests
	{
		[Test]
		public void NoConstraints_ReturnsCorrectFullLine()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1), new Position(2, 2), new Position(3, 2), new Position(4, 3) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithPositiveLimit_ReturnsLineWithProperLength()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			int limit = 3;

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, limit);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1), new Position(2, 2), new Position(3, 2) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithLimitZero_ReturnsEmptyLine()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			int limit = 0;

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, limit);

			result.Should().BeEquivalentTo(new Position[]{},
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithObstacleAndAllowsFinishOnBlocker_ReturnsLineFinishingOnBlocker()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			Position blocker = new Position(2,2);

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, position => position != blocker, true);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1), new Position(2, 2) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithObstacleAndDoesNotAllowFinishOnBlocker_ReturnsLineFinishingBeforeBlocker()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			Position blocker = new Position(2,2);

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, position => position != blocker, false);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithObstacleAndLimitAndLimitIsBeforeObstacle_ReturnsLimitedLine()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			Position blocker = new Position(2,2);

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, 1, position => position != blocker);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1) },
				options => options.WithStrictOrdering());
		}

		[Test]
		public void LineWithObstacleAndLimitAndLimitIsAfterObstacle_ReturnsLineTerminatedOnObstacle()
		{
			Position start = new Position(1,1);
			Position end = new Position(4,3);
			var creator = new BresenhamLineCreator();
			Position blocker = new Position(2,2);

			IList<Position> result = creator.GetRasterLine(start.x, start.y, end.x, end.y, 500, position => position != blocker, true);

			result.Should().BeEquivalentTo(new[]{ new Position(1, 1), new Position(2, 2) },
				options => options.WithStrictOrdering());
		}

		/// <summary>
		/// ....e..
		/// ..#/...
		/// .s-....
		/// .......
		/// </summary>
		[Test]
		public void Permissive_WallCanBeAvoidedBySecondaryChoices_ReturnsCorrectFullLine()
		{
			Position start = new Position(1, 1);
			Position end = new Position(4, 3);
			var creator = new BresenhamLineCreator();

			Position blocker = new Position(2,2);
			IList<Position> result = creator.GetRasterLinePermissive(start.x, start.y, end.x, end.y, position => position != blocker, -1);

			result.Should().BeEquivalentTo(new[] { new Position(1, 1), new Position(2, 1), new Position(3, 2), new Position(4, 3) },
				options => options.WithStrictOrdering());
		}

		/// <summary>
		/// .....e.
		/// ..###..
		/// .s-/...
		/// .......
		/// </summary>
		[Test]
		public void Permissive_WallCanBeAvoidedBySecondaryChoicesButLeadsToFailure_ReturnsCorrectLine()
		{
			Position start = new Position(1, 1);
			Position end = new Position(5, 3);
			var creator = new BresenhamLineCreator();

			var blockers = new[]{new Position(2, 2), new Position(3, 2), new Position(4, 2)}.ToList();
			IList<Position> result = creator.GetRasterLinePermissive(start.x, start.y, end.x, end.y, position => !blockers.Contains(position), -1);

			result.Should().BeEquivalentTo(new[] { new Position(1, 1), new Position(2, 1), new Position(3, 1), new Position(4, 2) },
				options => options.WithStrictOrdering());
		}

		/// <summary>
		/// .......
		/// .......
		/// .s--#e.
		/// .......
		/// </summary>
		[Test]
		public void Permissive_HorizontalLineBlocked_ReturnsCorrectLine()
		{
			Position start = new Position(1, 1);
			Position end = new Position(5, 1);
			var creator = new BresenhamLineCreator();

			var blockers = new[]{new Position(4, 1)}.ToList();
			IList<Position> result = creator.GetRasterLinePermissive(start.x, start.y, end.x, end.y, position => !blockers.Contains(position), -1);

			result.Should().BeEquivalentTo(new[] { new Position(1, 1), new Position(2, 1), new Position(3, 1), new Position(4, 1) },
				options => options.WithStrictOrdering());
		}

		/// <summary>
		/// .e......
		/// .#......
		/// .s......
		/// ........
		/// </summary>
		[Test]
		public void Permissive_VerticalLineBlocked_ReturnsCorrectLine()
		{
			Position start = new Position(1, 1);
			Position end = new Position(1, 3);
			var creator = new BresenhamLineCreator();

			var blockers = new[]{new Position(1, 2)}.ToList();
			IList<Position> result = creator.GetRasterLinePermissive(start.x, start.y, end.x, end.y, position => !blockers.Contains(position), -1);

			result.Should().BeEquivalentTo(new[] { new Position(1, 1), new Position(1, 2) },
				options => options.WithStrictOrdering());
		}
	}
}