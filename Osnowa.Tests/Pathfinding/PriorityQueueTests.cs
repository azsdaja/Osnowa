namespace Osnowa.Tests.Pathfinding
{
	using System;
	using System.Diagnostics;
	using C5;
	using NUnit.Framework;

	[TestFixture]
	public class PriorityQueueLoadTests
	{
		[Test]
		public void CompareThreeQueuesPerformance()
		{
			var rng = new RandomNumberGenerator(456);
			int size = 100000;
			int attempts = 10;

			int[] input = new int[size];
			for (int i = 0; i < size; i++)
			{
				input[i] = rng.Next(size);
			}

			var oldQueueComparer =
				FunctionalComparer<PQIndexedObject>.Create((first, second) => first.Priority.CompareTo(second.Priority));
			var spatialAStarQueue = new PriorityQueue<PQIndexedObject>(oldQueueComparer, size);
			var fastQueue = new FastPriorityQueue<FPQNode>(size);
			var intervalHeapQueue = new IntervalHeap<int>();

			var stopwatchOverall = Stopwatch.StartNew();
			for (int i = 0; i < attempts; i++)
			{
				spatialAStarQueue.Clear();
				var stopwatch = Stopwatch.StartNew();
				for (int itemIndex = 0; itemIndex < size; itemIndex++)
				{
					var oldItem = new PQIndexedObject { Priority = itemIndex};
					spatialAStarQueue.Push(oldItem);
				}
				Console.WriteLine(" Spatial A* queue " + stopwatch.ElapsedMilliseconds + " ms for " + attempts + " times " +  size + " items.");
			}

			Console.WriteLine(" Spatial A* queue total: " + stopwatchOverall.ElapsedMilliseconds + " ms for " + size + " items.");
			stopwatchOverall.Restart();

			for (int i = 0; i < attempts; i++)
			{
				intervalHeapQueue = new IntervalHeap<int>();
				var stopwatch = Stopwatch.StartNew();
				for (int itemIndex = 0; itemIndex < size; itemIndex++)
				{
					intervalHeapQueue.Add(itemIndex);
				}
				Console.WriteLine(" Interval Heap " + stopwatch.ElapsedMilliseconds + " ms for " + attempts + " times " +  size + " items.");
			}

			Console.WriteLine(" Interval Heap total: " + stopwatchOverall.ElapsedMilliseconds + " ms for " + size + " items.");
			stopwatchOverall.Restart();

			for (int i = 0; i < attempts; i++)
			{
				fastQueue.Clear();
				var stopwatch = Stopwatch.StartNew();
				for (int itemIndex = 0; itemIndex < size; itemIndex++)
				{
					var newItem = new FPQNode() { Priority = itemIndex };
					fastQueue.Enqueue(newItem, itemIndex);
				}
				Console.WriteLine(" Fast queue " + stopwatch.ElapsedMilliseconds + " ms for " + size + " items.");
			}
			Console.WriteLine(" Fast queue total: " + stopwatchOverall.ElapsedMilliseconds + " ms for " + attempts + " times " + size + " items.");

		}
	}

	public class FPQNode : FastPriorityQueueNode
	{
	}

	public class PQIndexedObject : IIndexedObject
	{
		public int QueueIndex { get; set; }
		public int Priority { get; set; }
	}
}