/*
The MIT License

Copyright (c) 2010 Christoph Husse

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace Libraries.SpatialAStar.SpatialAStar.Algorithm
{
	using System;
	using System.Collections.Generic;
	using PathPlannersLib;

	/// <summary>
    /// Uses about 50 MB to store a 1024x1024 grid.
    /// </summary>
    public class SpatialAStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
    {
        private OpenCloseMap m_ClosedSet;
        private OpenCloseMap m_OpenSet;
        private PriorityQueue<PathNode> m_OrderedOpenSet;
        private PathNode[,] m_CameFrom;
        private OpenCloseMap m_RuntimeGrid;
        private PathNode[,] m_SearchSpace;

	    public static long TotalClosedNodes;
	    public static long TotalSteps;

        public TPathNode[,] SearchSpace { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected class PathNode : IPathNode<TUserContext>, IComparer<PathNode>, IIndexedObject
        {
            public static readonly PathNode Comparer = new PathNode(0, 0, default(TPathNode));

            public TPathNode UserContext { get; internal set; }
            public float G { get; internal set; }
            public float H { get; internal set; }
            public float F { get; internal set; }
            public int QueueIndex { get; set; }

	        public float Cost { get { return UserContext.Cost; }}

	        public Boolean IsWalkable(TUserContext inContext)
            {
                return UserContext.IsWalkable(inContext);
            }

            public int X { get; internal set; }
            public int Y { get; internal set; }

            public int Compare(PathNode x, PathNode y)
            {
                if (x.F < y.F)
                    return -1;
                else if (x.F > y.F)
                    return 1;

                return 0;
            }

            public PathNode(int inX, int inY, TPathNode inUserContext)
            {
                X = inX;
                Y = inY;
                UserContext = inUserContext;
            }
        }

        public SpatialAStar(TPathNode[,] inGrid)
        {
            SearchSpace = inGrid;
            Width = inGrid.GetLength(0);
            Height = inGrid.GetLength(1);
            m_SearchSpace = new PathNode[Width, Height];
            m_ClosedSet = new OpenCloseMap(Width, Height);
            m_OpenSet = new OpenCloseMap(Width, Height);
            m_CameFrom = new PathNode[Width, Height];
            m_RuntimeGrid = new OpenCloseMap(Width, Height);
            m_OrderedOpenSet = new PriorityQueue<PathNode>(PathNode.Comparer, 200);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (inGrid[x, y] == null)
                        throw new ArgumentNullException();

                    m_SearchSpace[x, y] = new PathNode(x, y, inGrid[x, y]);
                }
            }
        }

        protected virtual float Heuristic(PathNode inStart, PathNode inEnd)
        {
	        // return Math.Max(Math.Abs(inStart.X - inEnd.x), Math.Abs(inStart.Y - inEnd.y));

			// performance: Sqrt is not slower than code above! but extracting deltaX and deltaY gives 5% gain in whole pathfinding 
	        int deltaX = inStart.X - inEnd.X;
	        int deltaY = inStart.Y - inEnd.Y;
			// performance: you can try multiplying this result by a modifier to get different ratio of execution_time/path_accuracy
	        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

	    private static readonly float SQRT_2 = 1.42f;
	    private readonly PathNode[] _neighborNodes = new PathNode[8]; // performance: makes pathfinding 3% slower, 
																	  // but saves memory in comparison to instantiation each time when needed

	    protected virtual float NeighborDistance(PathNode inStart, PathNode inEnd)
        {
			// return 1;
			// performance: return 1 would be more valid for the game mechanics, but for roads it would generate 
			// very straight ones (why?) It's not faster anyway! (why?).

            int diffX = Math.Abs(inStart.X - inEnd.X);
            int diffY = Math.Abs(inStart.Y - inEnd.Y);

            switch (diffX + diffY)
            {
                case 1: return 1;
                case 2: return SQRT_2;
                case 0: return 0;
                default:
                    throw new ApplicationException();
            }
        }

        //private List<Int64> elapsed = new List<long>();

        /// <summary>
        /// Returns null, if no path is found. Start- and End-Node are included in returned path. The user context
        /// is passed to IsWalkable().
        /// </summary>
        public LinkedList<TPathNode> Search(Point inStartNode, Point inEndNode, TUserContext inUserContext, out int nodesOpen, out int nodesClosed)
        {
	        nodesOpen = 0;
			nodesClosed = 0;

			if (inStartNode.X < 0 || inStartNode.Y < 0 || inEndNode.X < 0 || inEndNode.Y < 0
	            || inStartNode.X >= m_SearchSpace.GetLength(0) || inStartNode.Y >= m_SearchSpace.GetLength(1)
	            || inEndNode.X >= m_SearchSpace.GetLength(0) || inEndNode.X >= m_SearchSpace.GetLength(1))
	        {
				return null;
			}


			PathNode startNode = m_SearchSpace[inStartNode.X, inStartNode.Y];
            PathNode endNode = m_SearchSpace[inEndNode.X, inEndNode.Y];

            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

	        if (startNode == endNode)
	        {
		        return new LinkedList<TPathNode>(new TPathNode[] { startNode.UserContext });
	        }

            Array.Clear(_neighborNodes, 0, 8);

            m_ClosedSet.Clear();
            m_OpenSet.Clear();
            m_RuntimeGrid.Clear();
            m_OrderedOpenSet.Clear();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    m_CameFrom[x, y] = null;
                }
            }

            startNode.G = 0;
            startNode.H = Heuristic(startNode, endNode);
            startNode.F = startNode.H;

            m_OpenSet.Add(startNode);
	        ++nodesOpen;
            m_OrderedOpenSet.Push(startNode);

            m_RuntimeGrid.Add(startNode);

            int nodes = 0;


            while (!m_OpenSet.IsEmpty)
            {
                PathNode poppedNode = m_OrderedOpenSet.Pop();

                if (poppedNode == endNode)
                {
                   // watch.Stop();

                    //elapsed.Add(watch.ElapsedMilliseconds);
					
                    LinkedList<TPathNode> result = ReconstructPath(m_CameFrom, m_CameFrom[endNode.X, endNode.Y]);

                    result.AddLast(endNode.UserContext);

	                TotalSteps += result.Count;
	                TotalClosedNodes += m_ClosedSet.Count;
	                nodesClosed = m_ClosedSet.Count;
					return result;
                }

                m_OpenSet.Remove(poppedNode);
                m_ClosedSet.Add(poppedNode);

                StoreNeighborNodes(poppedNode, _neighborNodes);

                for (int i = 0; i < _neighborNodes.Length; i++)
                {
                    PathNode neighbour = _neighborNodes[i];
                    Boolean tentative_is_better;

                    if (neighbour == null)
                        continue;

                    if (!neighbour.UserContext.IsWalkable(inUserContext))
                        continue;

                    if (m_ClosedSet.Contains(neighbour))
                        continue;

                    nodes++;

	                float tentative_g_score = m_RuntimeGrid[poppedNode].G
	                                           + NeighborDistance(poppedNode, neighbour)
												+ neighbour.UserContext.Cost; //+ poppedNode.UserContext.Cost; 
																			  // performance: same. wiki says use neighbour. with poppedNode maybe it's 
																			  // slightly nicer, but the roads are getting thicker. 
					Boolean wasAdded = false;

                    if (!m_OpenSet.Contains(neighbour))
                    {
						++nodesOpen;
						m_OpenSet.Add(neighbour);
                        tentative_is_better = true;
                        wasAdded = true;
                    }
                    else if (tentative_g_score < m_RuntimeGrid[neighbour].G) 
						// m_RuntimeGrid doesn't need to be initialized with infinity, because with unvisited not we will fall anyway to tentative_is_better
                    {
                        tentative_is_better = true;
                    }
                    else
                    {
                        tentative_is_better = false;
                    }

                    if (tentative_is_better)
                    {
                        m_CameFrom[neighbour.X, neighbour.Y] = poppedNode;

                        if (!m_RuntimeGrid.Contains(neighbour))
                            m_RuntimeGrid.Add(neighbour);

                        m_RuntimeGrid[neighbour].G = tentative_g_score;
	                    float heuristic = Heuristic(neighbour, endNode);
						m_RuntimeGrid[neighbour].H = heuristic;
                        m_RuntimeGrid[neighbour].F = m_RuntimeGrid[neighbour].G + heuristic;

                        if (wasAdded)
                            m_OrderedOpenSet.Push(neighbour);
                        else
                            m_OrderedOpenSet.Update(neighbour);
                    }
                }
            }

            return null;
        }

        private LinkedList<TPathNode> ReconstructPath(PathNode[,] came_from, PathNode current_node)
        {
            LinkedList<TPathNode> result = new LinkedList<TPathNode>();

            ReconstructPathRecursive(came_from, current_node, result);

            return result;
        }

        private void ReconstructPathRecursive(PathNode[,] came_from, PathNode current_node, LinkedList<TPathNode> result)
        {
            PathNode item = came_from[current_node.X, current_node.Y];

            if (item != null)
            {
                ReconstructPathRecursive(came_from, item, result);

                result.AddLast(current_node.UserContext);
            }
            else
                result.AddLast(current_node.UserContext);
        }

        private void StoreNeighborNodes(PathNode inAround, PathNode[] inNeighbors)
        {
            int x = inAround.X;
            int y = inAround.Y;
		
			if ((x > 0) && (y > 0))
                inNeighbors[0] = m_SearchSpace[x - 1, y - 1];
            else
                inNeighbors[0] = null;

			if (x > 0)
				inNeighbors[1] = m_SearchSpace[x - 1, y];
			else
				inNeighbors[1] = null;

			if ((x > 0) && (y < Height - 1))
				inNeighbors[2] = m_SearchSpace[x - 1, y + 1];
			else
				inNeighbors[2] = null;

			if (y > 0)
                inNeighbors[3] = m_SearchSpace[x, y - 1];
            else
                inNeighbors[3] = null;

			if (y < Height - 1)
				inNeighbors[4] = m_SearchSpace[x, y + 1];
			else
				inNeighbors[4] = null;

			if ((x < Width - 1) && (y > 0))
                inNeighbors[5] = m_SearchSpace[x + 1, y - 1];
            else
                inNeighbors[5] = null;

            if (x < Width - 1)
                inNeighbors[6] = m_SearchSpace[x + 1, y];
            else
                inNeighbors[6] = null;

            if ((x < Width - 1) && (y < Height - 1))
                inNeighbors[7] = m_SearchSpace[x + 1, y + 1];
            else
                inNeighbors[7] = null;
		}

        private class OpenCloseMap
        {
            private PathNode[,] m_Map;
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Count { get; private set; }

            public PathNode this[int x, int y]
            {
                get
                {
                    return m_Map[x, y];
                }
            }

            public PathNode this[PathNode Node]
            {
                get
                {
                    return m_Map[Node.X, Node.Y];
                }

            }

            public bool IsEmpty
            {
                get
                {
                    return Count == 0;
                }
            }

            public OpenCloseMap(int inWidth, int inHeight)
            {
                m_Map = new PathNode[inWidth, inHeight];
                Width = inWidth;
                Height = inHeight;
            }

            public void Add(PathNode inValue)
            {
                PathNode item = m_Map[inValue.X, inValue.Y];

#if DEBUG
                if (item != null)
                    throw new ApplicationException();
#endif

                Count++;
                m_Map[inValue.X, inValue.Y] = inValue;
            }

            public bool Contains(PathNode inValue)
            {
                PathNode item = m_Map[inValue.X, inValue.Y];

                if (item == null)
                    return false;

#if DEBUG
                if (!inValue.Equals(item))
                    throw new ApplicationException();
#endif

                return true;
            }

            public void Remove(PathNode inValue)
            {
                PathNode item = m_Map[inValue.X, inValue.Y];

#if DEBUG
                if (!inValue.Equals(item))
                    throw new ApplicationException();
#endif

                Count--;
                m_Map[inValue.X, inValue.Y] = null;
            }

            public void Clear()
            {
                Count = 0;
				
				for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        m_Map[x, y] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Works just like Search, but fills pathfindingDebug array cells with 1 for open positions and 2 for closed positions.
        /// </summary>
		public LinkedList<TPathNode> DebugSearch(Point inStartNode, Point inEndNode, TUserContext inUserContext, int[,] pathfindingDebug)
        {
	        const int openCell = 1;
	        const int closedCell = 2;
	        
			if (inStartNode.X < 0 || inStartNode.Y < 0 || inEndNode.X < 0 || inEndNode.Y < 0
				|| inStartNode.X >= m_SearchSpace.GetLength(0) || inStartNode.Y >= m_SearchSpace.GetLength(1)
				|| inEndNode.X >= m_SearchSpace.GetLength(0) || inEndNode.X >= m_SearchSpace.GetLength(1))
				return null;


			PathNode startNode = m_SearchSpace[inStartNode.X, inStartNode.Y];
			PathNode endNode = m_SearchSpace[inEndNode.X, inEndNode.Y];

			if (startNode == endNode)
				return new LinkedList<TPathNode>(new TPathNode[] { startNode.UserContext });

			Array.Clear(_neighborNodes, 0, 8);

			m_ClosedSet.Clear();
			m_OpenSet.Clear();
			m_RuntimeGrid.Clear();
			m_OrderedOpenSet.Clear();

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					m_CameFrom[x, y] = null;
				}
			}

			startNode.G = 0;
			startNode.H = Heuristic(startNode, endNode);
			startNode.F = startNode.H;

			m_OpenSet.Add(startNode);
			pathfindingDebug[startNode.X, startNode.Y] = openCell;
			m_OrderedOpenSet.Push(startNode);

			m_RuntimeGrid.Add(startNode);

			int nodes = 0;

			while (!m_OpenSet.IsEmpty)
			{
				PathNode poppedNode = m_OrderedOpenSet.Pop();

				if (poppedNode == endNode)
				{
					LinkedList<TPathNode> result = ReconstructPath(m_CameFrom, m_CameFrom[endNode.X, endNode.Y]);

					result.AddLast(endNode.UserContext);
					pathfindingDebug[poppedNode.X, poppedNode.Y] = closedCell;

					TotalSteps += result.Count;
					TotalClosedNodes += m_ClosedSet.Count;
					return result;
				}

				m_OpenSet.Remove(poppedNode);
				m_ClosedSet.Add(poppedNode);
				pathfindingDebug[poppedNode.X, poppedNode.Y] = closedCell;

				StoreNeighborNodes(poppedNode, _neighborNodes);

				for (int i = 0; i < _neighborNodes.Length; i++)
				{
					PathNode neighbour = _neighborNodes[i];
					Boolean tentative_is_better;

					if (neighbour == null)
						continue;

					if (!neighbour.UserContext.IsWalkable(inUserContext))
						continue;

					if (m_ClosedSet.Contains(neighbour))
						continue;

					nodes++;

					float tentative_g_score = m_RuntimeGrid[poppedNode].G
											   + NeighborDistance(poppedNode, neighbour)
												+ neighbour.UserContext.Cost; //+ poppedNode.UserContext.Cost; 
																			  // performance: same. wiki says use neighbour. with poppedNode maybe it's 
																			  // slightly nicer, but the roads are getting thicker. 
					Boolean wasAdded = false;

					if (!m_OpenSet.Contains(neighbour))
					{
						m_OpenSet.Add(neighbour);
						pathfindingDebug[neighbour.X, neighbour.Y] = openCell;

						tentative_is_better = true;
						wasAdded = true;
					}
					else if (tentative_g_score < m_RuntimeGrid[neighbour].G)
					// m_RuntimeGrid doesn't need to be initialized with infinity, because with unvisited not we will fall anyway to tentative_is_better
					{
						tentative_is_better = true;
					}
					else
					{
						tentative_is_better = false;
					}

					if (tentative_is_better)
					{
						m_CameFrom[neighbour.X, neighbour.Y] = poppedNode;

						if (!m_RuntimeGrid.Contains(neighbour))
							m_RuntimeGrid.Add(neighbour);

						m_RuntimeGrid[neighbour].G = tentative_g_score;
						float heuristic = Heuristic(neighbour, endNode);
						m_RuntimeGrid[neighbour].H = heuristic;
						m_RuntimeGrid[neighbour].F = m_RuntimeGrid[neighbour].G + heuristic;

						if (wasAdded)
							m_OrderedOpenSet.Push(neighbour);
						else
							m_OrderedOpenSet.Update(neighbour);
					}
				}
			}

			return null;
		}
    }
}
