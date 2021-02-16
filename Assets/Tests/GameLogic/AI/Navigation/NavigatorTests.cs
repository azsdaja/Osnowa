namespace Tests.GameLogic.AI.Navigation
{
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using global::GameLogic;
	using global::GameLogic.AI.Navigation;
	using Moq;
	using NUnit.Framework;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Pathfinding;

	[TestFixture]
	public class NavigatorTests
	{
		[Test]
		public void GetNavigationData_TargetIsSameAsStart_ReturnsCorrectNavigationData()
		{
			var position = new Position(2,2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(gip => gip.IsWalkable(position) == true);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(), 
				Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(position, position);

			result.Destination.Should().Be(position);
			result.RemainingNodes.Should().BeNull();
			result.RemainingStepsInCurrentSegment.Should().BeEmpty();
		}

		[Test]
		public void GetNavigationData_TargetIsUnwalkable_ReturnsNull()
		{
			var target = new Position(2,2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(gip => gip.IsWalkable(target) == false);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(), 
				Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(new Position(0, 0), target);

			result.Should().BeNull();
		}

		[Test]
		public void GetNavigationData_TargetIsReachableByStraightLine_PathfinderIsNotUsedAndReturnsCorrectResult()
		{
			var start = new Position(0, 0);
			var target = new Position(3, 3);
			var pathfinder = new Mock<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(gip => gip.IsWalkable(It.IsAny<Position>()) == true);
			var bresenham = new BresenhamLineCreator();
			var navigator = new Navigator(pathfinder.Object, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), bresenham, Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(start, target);

			pathfinder.Verify(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()), Times.Never);
			result.Destination.Should().Be(target);
			result.RemainingNodes.Should().BeEquivalentTo(new[]{target});
			result.RemainingStepsInCurrentSegment.Should().BeEmpty();
		}

		[Test]
		// .j.t
		// j#..
		// j#..
		// s...
		public void GetNavigationData_TargetIsNotReachableByStraightLineButReachableByPathfinding_PathfinderIsUsedAndReturnsCorrectResult()
		{
			var start = new Position(0, 0);
			var target = new Position(3, 3);
			var pathfinderMock = new Mock<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(gip =>
													gip.IsWalkable(It.IsAny<Position>()) == true
													&& gip.IsWalkable(new Position(1,1)) == false
													&& gip.IsWalkable(new Position(1, 2)) == false);
			var jumpPointsFromPathfinder = new List<Position>{new Position(0,0), new Position(0,1), new Position(0,2),
																new Position(1,3), new Position(3,3)};
			pathfinderMock.Setup(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()))
				.Returns(new PathfindingResponse(jumpPointsFromPathfinder));
			var bresenham = new BresenhamLineCreator();
			var navigator = new Navigator(pathfinderMock.Object, gridInfoProvider, new NaturalLineCalculator(bresenham), bresenham, Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(start, target);

			pathfinderMock.Verify(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()), Times.Once);
			result.Destination.Should().Be(target);
			result.RemainingNodes[0].Should().Be(new Position(0, 2));
			IList<Position> expectedNodes // (0,1) is redundant (because of specific of current JPS implementation) and should be pruned.
				= new List<Position>(new[] { new Position(0, 2), new Position(1, 3), new Position(3, 3)  });
			result.RemainingNodes.Should().BeEquivalentTo(expectedNodes, options => options.WithStrictOrderingFor(position => position));
			result.RemainingStepsInCurrentSegment.Should().BeEmpty();
		}

		[Test]
		// ..jn.. legend: s=start, j=jump point from pathfinder, t=target, #=wall, 
		// .j##t.         n=jump point and at the same time a natural replacement for other jump point the one at (2,2)
		// s..#..
		public void GetNavigationData_PathfinderIsUsed_RemainingNodesAreCorrectWithNaturalFirstJumpPoint()
		{
			var start = new Position(0, 0);
			var target = new Position(4, 1);
			var pathfinderMock = new Mock<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(gip =>
													gip.IsWalkable(It.IsAny<Position>()) == true
													&& gip.IsWalkable(new Position(2,1)) == false
													&& gip.IsWalkable(new Position(3,1)) == false
													&& gip.IsWalkable(new Position(3,0)) == false);
			var jumpPointsFromPathfinder = new List<Position>
			{
				new Position(0,0),
				new Position(1, 1), // should be pruned because it's redundant
				new Position(2, 2), // when actor start navigating towards this node, it will be changed to it's natural jump point (3,2) and
									  // next jump point will be pruned because it's the same.
				new Position(3, 2),
				new Position(4,1)
			};
			pathfinderMock.Setup(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()))
				.Returns(new PathfindingResponse(jumpPointsFromPathfinder));
			var bresenham = new BresenhamLineCreator();
			var navigator = new Navigator(pathfinderMock.Object, gridInfoProvider, new NaturalLineCalculator(bresenham), 
				bresenham, Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(start, target);

			pathfinderMock.Verify(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()), Times.Once);
			result.Destination.Should().Be(target);
			IList<Position> expectedNodes
				= new List<Position>(new[] { new Position(2, 2), new Position(3, 2), new Position(4, 1)  });
			result.RemainingNodes.Should().BeEquivalentTo(expectedNodes, options => options.WithStrictOrderingFor(position => position));
			result.RemainingStepsInCurrentSegment.Should().BeEmpty();
		}

		[Test]
		public void GetNavigationData_TargetIsNotReachableByStraightLineNorByPathfinding_ReturnsNull()
		{
			var start = new Position(0, 0);
			var target = new Position(3, 3);
			var pathfinder = Mock.Of<IPathfinder>(p => 
				p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()) 
				== new PathfindingResponse(PathfindingResult.FailureTargetUnreachable));
			IGrid grid = Mock.Of<IGrid>(g =>
													g.IsWalkable(It.IsAny<Position>()) == true
													&& g.IsWalkable(new Position(1,1)) == false
													&& g.IsWalkable(new Position(1, 2)) == false);
			var bresenham = new BresenhamLineCreator();
			var navigator = new Navigator(pathfinder, grid, new NaturalLineCalculator(bresenham), bresenham, Mock.Of<IUiFacade>());

			NavigationData result = navigator.GetNavigationData(start, target);

			Mock.Get(pathfinder).Verify(p => p.FindJumpPointsWithJps(It.IsAny<Position>(), It.IsAny<Position>(), It.IsAny<JpsMode>()), Times.Once);
			result.Should().BeNull();
		}

		[Test]
		public void ResolveNextStep_IsAtDestination_ReturnsSuccess()
		{
			var currentPosition = new Position(2, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == true);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(),
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				Destination = currentPosition
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.Finished);
		}

		[Test]
		public void ResolveNextStep_NextStepIsWalkableDestination_ReturnsInProgressWithNextStepAndUpcomingStepsAreCorrect()
		{
			var currentPosition = new Position(2, 2);
			var destination = new Position(3, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == true);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(),
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				Destination = destination,
				RemainingStepsInCurrentSegment = new Stack<Position>(new[]{ destination }),
				RemainingNodes = new List<Position>{destination},
				LastStep = currentPosition
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgress);
			nextStep.Should().Be(destination);
			navigationData.RemainingStepsInCurrentSegment.Should().BeEmpty();
			navigationData.LastStep.Should().Be(destination);
			navigationData.RemainingNodes.Should().BeEmpty();
		}

		[Test]
		public void ResolveNextStep_NextStepIsDestinationButUnwalkable_ReturnsUnreachable()
		{
			var currentPosition = new Position(2, 2);
			var destination = new Position(3, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(destination) == false);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(),
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				Destination = destination,
				RemainingStepsInCurrentSegment = new Stack<Position>(new[] { destination, currentPosition })
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.Unreachable);
		}

		[Test]
		public void ResolveNextStep_NextStepIsNextNode_ReturnsInProgressWithNextStepAndNavigationDataIsCorrect()
		{
			var currentPosition = new Position(2, 2);
			var nextNode = new Position(3, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == true);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(),
				Mock.Of<IUiFacade>());
			var nodesToVisit = new[] {nextNode, new Position(213, 34254),};
			var navigationData = new NavigationData
			{
				RemainingNodes = nodesToVisit.ToList(),
				RemainingStepsInCurrentSegment = new Stack<Position>(new[]{ nextNode }),
				Destination = new Position(4,2),
				LastStep = currentPosition
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgress);
			nextStep.Should().Be(nextNode);
			navigationData.RemainingStepsInCurrentSegment.Should().BeEmpty();
			navigationData.RemainingNodes.Should().BeEquivalentTo(nodesToVisit.Skip(1));
		}

		[Test]
		public void ResolveNextStep_ActorWasDisplacedToPositionNeighboringLastStepButNotNextStep_ReturnsInProgressWithMovementToLastStepAndStackIsCorrect()
		{
			var currentPosition = new Position(-1, 1);
			var lastStepPosition = new Position(0, 1);
			var nextPosition = new Position(1, 1);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == true);
			var navigator = new Navigator(pathfinder, gridInfoProvider, Mock.Of<INaturalLineCalculator>(), Mock.Of<IRasterLineCreator>(),
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				RemainingStepsInCurrentSegment = new Stack<Position>(new[]{ nextPosition, lastStepPosition }),
				Destination = nextPosition,
				LastStep = lastStepPosition,
				RemainingNodes = new []{nextPosition}.ToList()
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgress);
			nextStep.Should().Be(lastStepPosition);
			var expectedSteps = new Stack<Position>(new[] { nextPosition });
			navigationData.RemainingStepsInCurrentSegment.
				Should().BeEquivalentTo(expectedSteps, options => options.WithStrictOrderingFor(position => position));
		}

		[Test]
		public void ResolveNextStep_ActorWasDisplacedToPositionNeighboringLastStepAndNextStep_ReturnsInProgressWithMovementToNextStepAndStackIsEmpty()
		{
			var currentPosition = new Position(1, 1);
			var lastStepPosition = new Position(1, 0);
			var nextPosition = new Position(2, 0);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(It.IsAny<Position>()) == true);
			var bresenham = new BresenhamLineCreator();
			var navigator = new Navigator(pathfinder, gridInfoProvider, new NaturalLineCalculator(bresenham), bresenham,
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				RemainingStepsInCurrentSegment = new Stack<Position>(new[]{ nextPosition }),
				Destination = nextPosition,
				RemainingNodes = new[]{ nextPosition }.ToList(),
				LastStep = lastStepPosition
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgress);
			nextStep.Should().Be(nextPosition);
			navigationData.RemainingStepsInCurrentSegment.
				Should().BeEmpty();
		}

		[Test]
		public void ResolveNextStep_NextStepIsUnwalkable_RecalculatesNavigationDataAndReturnsResultForNewPath()
		{
			var currentPosition = new Position(2, 2);
			var nextStepOnStack = new Position(3, 2);
			var alternativeNextStep = new Position(3, 3);
			var destination = new Position(4, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p =>
				p.IsWalkable(It.IsAny<Position>()) == true
				&& p.IsWalkable(nextStepOnStack) == false
				);
			var bresenham = new BresenhamLineCreator();
			var navigatorMockReal = new Mock<Navigator>(pathfinder, gridInfoProvider, new NaturalLineCalculator(bresenham), 
				bresenham, Mock.Of<IUiFacade>()){CallBase = true};
			var currentNavigationData = new NavigationData
			{
				Destination = destination,
				RemainingStepsInCurrentSegment = new Stack<Position>(new[] { destination, nextStepOnStack })
			};
			var recalculatedNavigationData = new NavigationData
			{
				Destination = destination,
				RemainingNodes = new[]{ alternativeNextStep, destination }.ToList(),
				RemainingStepsInCurrentSegment = new Stack<Position>(new []{alternativeNextStep }),
				LastStep = currentPosition
			};
			navigatorMockReal.Setup(n => n.GetNavigationData(currentPosition, destination)).Returns(recalculatedNavigationData);

			Position nextStep;
			NavigationResult result = navigatorMockReal.Object.ResolveNextStep(currentNavigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgressWithRecalculation);
			var expectedNavigationData = new NavigationData
			{
				Destination = destination,
				RemainingNodes = new[] { destination }.ToList(),
				RemainingStepsInCurrentSegment = new Stack<Position>(),
				LastStep = alternativeNextStep
			};
			currentNavigationData.Should().BeEquivalentTo(expectedNavigationData);
			nextStep.Should().Be(alternativeNextStep);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ResolveNextStep_NextStepIsUnwalkableAndRecalculatingFails_ReturnsUnreachable(bool nextStepIsDestination)
		{
			var currentPosition = new Position(2, 2);
			var destination = new Position(4, 2);
			Position plannedNextStep = nextStepIsDestination ? destination : new Position(3, 2);
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			var bresenham = new BresenhamLineCreator();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => p.IsWalkable(plannedNextStep) == false);
			var navigator = new Navigator(pathfinder, gridInfoProvider, new NaturalLineCalculator(bresenham), bresenham,
				Mock.Of<IUiFacade>());
			var navigationData = new NavigationData
			{
				Destination = destination,
				RemainingStepsInCurrentSegment = new Stack<Position>(new[] { plannedNextStep, currentPosition })
			};

			Position nextStep;
			NavigationResult result = navigator.ResolveNextStep(navigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.Unreachable);
		}

		private static readonly object[] CorruptedNavigationDataCases =
		{
			new NavigationData
							{
								Destination = new Position(4, 3),
								RemainingStepsInCurrentSegment = new Stack<Position>(),
								RemainingNodes = null, // culprit
								LastStep = new Position(1,1)
							},
			new NavigationData
							{
								Destination = new Position(4, 3),
								RemainingStepsInCurrentSegment = null, // culprit
								RemainingNodes = new[]{ new Position(2, 2), new Position(4, 3) }.ToList(),
								LastStep = new Position(1,1)
							},
			new NavigationData
							{
								Destination = new Position(4, 3),
								RemainingStepsInCurrentSegment = new Stack<Position>(),
								RemainingNodes = new[]{ new Position(2, 2), new Position(4, 3) }.ToList(),
								LastStep = Position.Zero // culprit
							},
		};

		/// <summary>
		/// ....t.
		/// ..##..
		/// .s.j..
		/// ......
		/// </summary>
		/// <param name="inputNavigationData"></param>
		[TestCaseSource(nameof(CorruptedNavigationDataCases))]
		public void ResolveNextStep_NavigationDataIsCorrupted_RecalculationIsUsed(NavigationData inputNavigationData)
		{
			IPathfinder pathfinder = Mock.Of<IPathfinder>();
			IGrid gridInfoProvider = Mock.Of<IGrid>(p => 
			p.IsWalkable(It.IsAny<Position>()) == true
			&& p.IsWalkable(new Position(2,2)) == false
			&& p.IsWalkable(new Position(3,2)) == false // two walls, so that no recalculated nodes are skipped
			); 
			var bresenham = new BresenhamLineCreator();
			var navigatorMockReal = new Mock<Navigator>(pathfinder, gridInfoProvider, new NaturalLineCalculator(bresenham),
									bresenham, Mock.Of<IUiFacade>())
									{ CallBase = true };
			Position currentPosition = new Position(1,1);
			List<Position> recalculatedRemainingNodes = new[]{new Position(3,1), new Position(4,3)}.ToList();
			var recalculatedNavigationData = new NavigationData
			{
				Destination = recalculatedRemainingNodes[1],
				RemainingNodes = recalculatedRemainingNodes,
				RemainingStepsInCurrentSegment = new Stack<Position>(),
				LastStep = currentPosition
			};
			navigatorMockReal.Setup(n => n.GetNavigationData(currentPosition, recalculatedRemainingNodes[1]))
						     .Returns(recalculatedNavigationData);

			Position nextStep;
			NavigationResult result = navigatorMockReal.Object.ResolveNextStep(inputNavigationData, currentPosition, out nextStep);

			result.Should().Be(NavigationResult.InProgressWithRecalculation);
			var expectedNavigationData = new NavigationData
			{
				Destination = recalculatedRemainingNodes[1],
				RemainingNodes = recalculatedRemainingNodes,
				RemainingStepsInCurrentSegment = new Stack<Position>(new[] { recalculatedRemainingNodes[0] }),
				LastStep = new Position(2,1)
			};
			inputNavigationData.Should().BeEquivalentTo(expectedNavigationData);
			nextStep.Should().Be(new Position(2, 1));
		}
	}
}