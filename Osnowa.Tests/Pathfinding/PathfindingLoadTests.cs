namespace Osnowa.Tests.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime;
	using NUnit.Framework;

	[TestFixture]
	public class PathfindingLoadTests
	{
		[Test]
		public void PrintTestedMaps()
		{
			int xSize = 500;
			int ySize = 500;
			var density = TestGridInfoProvider.GridDensity.Medium;
			bool hasWalls = true;
			int seed = 333;

			var gip = new TestGridInfoProvider(xSize, ySize, density, hasWalls, seed);

			string header = String.Format("xSize: {0}, ySize: {1}, density: {2}, walls: {3}, seed: {4}",
				xSize, ySize, density, hasWalls, seed);
			Console.WriteLine(header);
			Console.Write(gip.Walkability());
			Console.WriteLine();
		}

		//[Ignore("This test should be run with DotMemory Unit framework (free)")]
		[DotMemoryUnit(CollectAllocations = true)]
		[Test]
		public void LoadTest_PathfindingPerformanceAndMemoryOverhead()
		{
			int xSize = 500;
			int ySize = 500;
			var density = TestGridInfoProvider.GridDensity.Medium;
			bool hasWalls = true;
			int rngSeed = 333;
			int trailsCount = 100;

			var gip = new TestGridInfoProvider(xSize, ySize, density, hasWalls, rngSeed);
			var rng = new RandomNumberGenerator(rngSeed);

			IRasterLineCreator bresenham = new BresenhamLineCreator();
			var pathfinder = new Pathfinder(gip, new NaturalLineCalculator(bresenham), bresenham);
			var stopwatch = new Stopwatch();

			var trailsToTest = new Dictionary<Position, Position>();
			for (int i = 0; i < trailsCount; i++)
			{
				Position first = GetRandomWalkablePosition(rng, gip);
				Position second = GetRandomWalkablePosition(rng, gip);
				trailsToTest[first] = second;
			}

			Func<Position, Position, PathfindingResult>[] findersToTest = {
				pathfinder.FindJumpPointsWithJps,
				pathfinder.FindJumpPointsWithSpatialAstar,
			};

			MemoryCheckPoint lastCheckPoint = new MemoryCheckPoint();// = dotMemory.Check();
			if (GC.TryStartNoGCRegion(240111000))
			{
				lastCheckPoint = RunPathfindingAndPrintResults(xSize, ySize, density, hasWalls, trailsCount, rngSeed, findersToTest, stopwatch, trailsToTest, 
					lastCheckPoint);
				if (GCSettings.LatencyMode == GCLatencyMode.NoGCRegion)
					GC.EndNoGCRegion();
			}
		}

		private static MemoryCheckPoint RunPathfindingAndPrintResults(int xSize, int ySize, TestGridInfoProvider.GridDensity density, bool hasWalls,
			int trailsCount, int seed, Func<Position, Position, PathfindingResult>[] findersToTest, Stopwatch stopwatch, Dictionary<Position, Position> trails, 
			MemoryCheckPoint lastCheckPoint)
		{
				foreach (Func<Position, Position, PathfindingResult> finder in findersToTest)
				{
					Console.WriteLine("Method name;" + finder.Method.Name);
					Console.WriteLine("xSize;" + xSize);
					Console.WriteLine("ySize;" + ySize);
					Console.WriteLine("density;" + density);
					Console.WriteLine("hasWalls;" + hasWalls);
					Console.WriteLine("trailsCount;" + trailsCount);
					Console.WriteLine("seed;" + seed);
					stopwatch.Start();
					int pathNotFoundCount = 0;
					int totalStepsCount = 0;
					foreach (var startToEnd in trails)
					{
						List<Position> jumpPoints = finder(startToEnd.Key, startToEnd.Value).Positions;
						if (jumpPoints != null)
							totalStepsCount += jumpPoints.Count;
						else
						{
							++pathNotFoundCount;
						}
					}
					stopwatch.Stop();
					Console.WriteLine("total time;" + stopwatch.ElapsedMilliseconds);
					Console.WriteLine("paths found;" + (trailsCount - pathNotFoundCount));
					Console.WriteLine("paths not found;" + pathNotFoundCount);
					Console.WriteLine("total count of steps to follow;" + totalStepsCount);
					stopwatch.Reset();
/*
					var previousCheckPoint = lastCheckPoint;
					lastCheckPoint = dotMemory.Check(memory =>
					{
						var traffic = memory.GetTrafficFrom(previousCheckPoint);
						var difference = memory.GetDifference(previousCheckPoint);

						Console.WriteLine("DIFFERENCE — NEW OBJECTS COUNT;" + difference.GetNewObjects().ObjectsCount);
						Console.WriteLine("DIFFERENCE — NEW OBJECTS SIZE IN BYTES;" + difference.GetNewObjects().SizeInBytes);
						Console.WriteLine("TRAFFIC — ALLOCATED OBJECTS COUNT;" + traffic.AllocatedMemory.ObjectsCount);
						Console.WriteLine("TRAFFIC — ALLOCATED SIZE IN BYTESl" + traffic.AllocatedMemory.SizeInBytes);
						Console.WriteLine("TRAFFIC — COLLECTED OBJECTS COUNT;" + traffic.CollectedMemory.ObjectsCount);
						Console.WriteLine("TRAFFIC — COLLECTED SIZE IN BYTESl" + traffic.CollectedMemory.SizeInBytes);
						Console.WriteLine();
						Console.WriteLine("NEW OBJECTS WITH COUNT OVER 50:");
						Console.WriteLine("OBJECT NAME;COUNT;BYTES");
						var types = difference.GetNewObjects().GroupByType();
						foreach (var tmi in types)
						{
							if (tmi.ObjectsCount < 50) continue;
							string full = String.Format("{0};{1};{2}", tmi.Type.ToString(), tmi.ObjectsCount,
								tmi.SizeInBytes);
							Console.WriteLine(full);
						}
						Console.WriteLine();
						Console.WriteLine("TRAFFIC OF TYPES WITH COUNT OVER 50:");
						Console.WriteLine("TYPE NAME;ALLOCATED MEMORY;COLLECTED MEMORY");

						var types2 = traffic.GroupByType();
						foreach (var tmi in types2)
						{
							if (tmi.AllocatedMemoryInfo.ObjectsCount > 50)
							{
								string full = String.Format("{0};{1};{2}", tmi.Type.ToString(), tmi.AllocatedMemoryInfo,
									tmi.CollectedMemoryInfo);
								Console.WriteLine(full);
							}
						}
						Console.WriteLine("-------------------------------------------------------------");
					});*/
				}
			return lastCheckPoint;
		}

		private Position GetRandomWalkablePosition(RandomNumberGenerator rng, TestGridInfoProvider gip)
		{
			Position position;
			do
			{
				position = rng.NextPosition(gip.Bounds);
			} while (!gip.IsWalkable(position));
			return position;
		}
	}
}