using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Osnowa.Osnowa.Core;
using Assets.Scripts.PCG.SpatialLogicForPCG;
using FluentAssertions;
using NUnit.Framework;
using Osnowa.Osnowa.Unity.UnityUtilities;

namespace Osnowa.Tests.GameLogic.MapGeneration
{
	[TestFixture]
	public class PlaceFinderTests
	{
		[Test]
		public void GrowRectangleFrom_CenterSurroundedByWalls_DoesNotGrowFromCenter()
		{
			string input = "###" + Environment.NewLine +
			               "#s#" + Environment.NewLine +
			               "###" + Environment.NewLine +
						   "#.#";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 2, 0, 1);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToBottom_ReturnsCorrectBounds()
		{
			string input = "###" + Environment.NewLine +
			               "#s#" + Environment.NewLine +
			               "#.#" + Environment.NewLine +
						   "###";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 1);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToRight_ReturnsCorrectBounds()
		{
			string input = "####" + Environment.NewLine +
			               "#s.#" + Environment.NewLine +
			               "####" + Environment.NewLine +
						   "";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 2);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToLeft_ReturnsCorrectBounds()
		{
			string input = "####" + Environment.NewLine +
			               "#.s#" + Environment.NewLine +
			               "####" + Environment.NewLine +
						   "";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 2);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToTop_ReturnsCorrectBounds()
		{
			string input = "###" + Environment.NewLine +
						   "#.#" + Environment.NewLine +
						   "#s#" + Environment.NewLine +
						   "###";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 1);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToTopLeft_ReturnsCorrectBounds()
		{
			string input = "####" + Environment.NewLine +
						   "#..#" + Environment.NewLine +
						   "#.s#" + Environment.NewLine +
						   "####";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 2);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToAllDirections_ReturnsCorrectBounds()
		{
			string input = "#######" + Environment.NewLine +
						   "#.....#" + Environment.NewLine +
						   "#.s...#" + Environment.NewLine +
						   "#.....#" + Environment.NewLine +
						   "#######";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(1, 1, 0, 5);
			bounds.Should().Be(expectedBounds);
		}

		[Test]
		public void GrowRectangleFrom_CanGrowToAllDirectionsButThereAreObstacles_ReturnsCorrectBounds()
		{
			string input = "#######" + Environment.NewLine +
						   "##...##" + Environment.NewLine +
						   "#.....#" + Environment.NewLine +
						   "#..s..#" + Environment.NewLine +
						   "#.....#" + Environment.NewLine +
						   "##...##" + Environment.NewLine +
						   "#######";
            char[,] data = ParseInput(input, out Position center);
			Func<Position, bool> canConstruct = CanConstruct(data);

			Bounds bounds = PlaceFinder.GrowRectangleFrom(center, canConstruct);

			Bounds expectedBounds = new Bounds(2, 1, 0, 3);
			bounds.Should().Be(expectedBounds);
		}

		/// <summary>
		/// Return input parsed to char array and start vector position ('s'). Position 0, 0 is the bottom-left corner of input and x grows to right.
		/// </summary>
		private char[,] ParseInput(string input, out Position start)
		{
			start = PositionUtilities.Min;
			List<string> rows = input.Split(new[]{ Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
			rows.Reverse();
			var result = new char[rows[0].Length, rows.Count];
			for (int x = 0; x < rows[0].Length; x++)
			{
				for (int y = 0; y < rows.Count; y++)
				{
					result[x, y] = rows[y][x];
					if (result[x, y] == 's')
					{
						start = new Position(x, y);
					}
				}
			}
			return result;
		}

		private static Func<Position, bool> CanConstruct(char[,] data)
		{
			return position => data[position.x, position.y] == '.';
		}
	}
}