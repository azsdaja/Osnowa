namespace GameLogic.AI.Navigation
{
	using System;
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	/// <summary>
	/// Used for navigating a single actor around the grid.
	/// </summary>
	[Serializable]
	public class NavigationData
	{
		[SerializeField, HideInInspector] private List<Position> _remainingNodes;
		[SerializeField, HideInInspector] private Position _destination;
		[SerializeField, HideInInspector] private Stack<Position> _remainingStepsInCurrentSegment;
		[SerializeField, HideInInspector] private Position _lastStep;

		/// <summary>
		/// Nodes to visit during the navigation towards the goal. They don't need to neighbor each other, 
		/// they rather define where the character turns.
		/// </summary>
		// todo/performance: use a stack? especially that removeat(0) eats O(n)!
		public List<Position> RemainingNodes
		{
			get { return _remainingNodes; }
			set { _remainingNodes = value; }
		}

		/// <summary>
		/// Positions to walk on in order to visit next node or target — step by step.
		/// </summary>
		public Stack<Position> RemainingStepsInCurrentSegment
		{
			get { return _remainingStepsInCurrentSegment; }
			set { _remainingStepsInCurrentSegment = value; }
		}

		/// <summary>
		/// Pathfinding destination.
		/// </summary>
		public Position Destination
		{
			get { return _destination; }
			set { _destination = value; }
		}

		/// <summary>
		/// Position of last step made during this navigation.
		/// </summary>
		public Position LastStep
		{
			get { return _lastStep; }
			set { _lastStep = value; }
		}

		public void OverwriteBy(NavigationData recalculatedNavigationData)
		{
			Destination = recalculatedNavigationData.Destination;
			RemainingStepsInCurrentSegment = recalculatedNavigationData.RemainingStepsInCurrentSegment;
			RemainingNodes = recalculatedNavigationData.RemainingNodes;
			LastStep = recalculatedNavigationData.LastStep;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"LastStep: {_lastStep}, Destination: {Destination}, RemainingStepsInCurrentSegment: " +
			       $"{(RemainingStepsInCurrentSegment == null ? "null" : string.Join(", ", RemainingStepsInCurrentSegment))}, RemainingNodes: " 
			       + $"{(RemainingNodes == null ? "null" : string.Join(", ", RemainingNodes))}";
		}
	}
}