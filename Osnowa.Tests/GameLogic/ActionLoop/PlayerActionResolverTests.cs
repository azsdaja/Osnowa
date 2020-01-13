namespace Osnowa.Tests.GameLogic.ActionLoop
{
/*
	[TestFixture]
	public class PlayerActionResolverTests
	{
		[Test]
		public void GetAction_PlayerInputIsEmpty_ReturnsNull()
		{
			IInputHolder Decision = Mock.Of<IInputHolder>(h => h.Decision == Decision.None);
			var resolver = new PlayerActionResolver(null, It.IsAny<IEntityDetector>(), Decision, Mock.Of<IActionFactory>());

			IGameAction gameAction = resolver.GetAction(It.IsAny<ActorData>());

			gameAction.Should().BeNull();
		}

		static readonly object[] DirectionCases = 
			{
			new object[] {Decision.MoveUp, 0, 1},
			new object[] {Decision.MoveLeft, -1, 0},
			new object[] {Decision.MoveDown, 0, -1},
			new object[] {Decision.MoveRight, 1, 0},
		};

		[TestCaseSource(nameof(DirectionCases))]
		public void GetAction_PlayerInputIsSetToMoveAndThereIsNoActorAtTarget_ReturnsCorrectMoveAction
																(Decision input, int expectedXDelta, int expectedYDelta)
		{
			IInputHolder Decision = Mock.Of<IInputHolder>(h => h.Decision == input);
			var resolver = new PlayerActionResolver(null, Mock.Of<IEntityDetector>(), Decision, 
				new ActionFactory(null, null, null, null, null, null, null, null, null, null, null, null, null));
			float expectedEnergyCost = 1.0f;

			var actionData = (JustMoveAction)resolver.GetAction(new SpecificActorData(0));

			actionData.Direction.Should().Be(new Position(expectedXDelta, expectedYDelta));
			actionData.EnergyCost.Should().Be(expectedEnergyCost);
		}

		[TestCaseSource(nameof(DirectionCases))]
		public void GetAction_PlayerInputIsSetToMoveAndThereIsFriendlyActorAtTarget_ReturnsCorrectDisplaceAction
			(Decision input, int expectedXDelta, int expectedYDelta)
		{
			IInputHolder Decision = Mock.Of<IInputHolder>(h => h.Decision == input);
			SpecificActorData actor = new SpecificActorData(0){/*Team = Team.Beasts#1#};
			SpecificActorData otherActor = Mock.Of<SpecificActorData>(/*d => d.Team == Team.Beasts#1#);
			var entityDetector = Mock.Of<IEntityDetector>(d => d.DetectActors(It.IsAny<Position>(), false) == new[]{otherActor});
			var resolver = new PlayerActionResolver(null, entityDetector, Decision, 
										new ActionFactory(null, null, null, null, null, null, null, null, null, null, null, null, null));
			float expectedEnergyCost = 1.0f;

			var actionData = (DisplaceAction)resolver.GetAction(actor);

			actionData.DisplacedEntity.Should().Be(otherActor);
			actionData.EnergyCost.Should().Be(expectedEnergyCost);
		}

		[TestCaseSource(nameof(DirectionCases))]
		public void GetAction_PlayerInputIsSetToMoveAndThereIsHostileActorAtTarget_ReturnsAttackAction
			(Decision input, int expectedXDelta, int expectedYDelta)
		{
			IInputHolder Decision = Mock.Of<IInputHolder>(h => h.Decision == input);
			var actor = new SpecificActorData(0){/*Team = Team.Beasts#1#};
			var otherActor = new SpecificActorData(0){/*Team = Team.Humans#1#};
			var entityDetector = Mock.Of<IEntityDetector>(d => d.DetectActors(It.IsAny<Position>(), false) == new[]{ otherActor });
			var resolver = new PlayerActionResolver(null, entityDetector, Decision,
				new ActionFactory(null, null, null, null, null, null, null, null, null, null, null, null, null));
			float expectedEnergyCost = 1.0f;

			var actionData = (AttackAction)resolver.GetAction(actor);

			actionData.ActorData.Should().Be(actor);
			actionData.AttackedEntity.Should().Be(otherActor);
			actionData.EnergyCost.Should().Be(expectedEnergyCost);
		}

		[Test]
		public void GetAction_GettingActionClearsInput()
		{
			var inputHolderMock = new Mock<IInputHolder>();
			inputHolderMock.SetupGet(h => h.Decision).Returns(Decision.MoveDown);
			var resolver = new PlayerActionResolver(null, Mock.Of<IEntityDetector>(), inputHolderMock.Object, 
				new ActionFactory(null, null, null, null, null, null, null, null, null, null, null, null, null));

			resolver.GetAction(new SpecificActorData(0));

			inputHolderMock.VerifySet(h => h.Decision = Decision.None);
		}
	}
*/
}