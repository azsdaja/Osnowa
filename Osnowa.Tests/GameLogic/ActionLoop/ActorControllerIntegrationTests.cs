namespace Osnowa.Tests.GameLogic.ActionLoop
{
/*
	[TestFixture]
	public class ActorControllerIntegrationTests
	{
		[Test]
		public void FriendlyActorsStaySideBySide_PlayerActorMovesTowardsOtherActor_TheyGetDisplacedAndAreUpdatedCorrectly()
		{
			var decision = Decision.MoveLeft;
			var initialPlayerPosition = new Position(5,10);
			var initialOtherActorPosition = new Position(4,10);
			var playerActor = new SpecificActorData(0)
			{
				ActorControlData = new ActorControlData(),
				LogicalPosition = initialPlayerPosition,
//				Team = Team.Beasts
			};
			var otherActor = new SpecificActorData(0)
			{
				LogicalPosition = initialOtherActorPosition,
//				Team = Team.Beasts
			};
			
			var entityDetector = Mock.Of<IEntityDetector>(d => d.DetectActors(otherActor.LogicalPosition, false) == new[]{otherActor});
			var actionEffectFactoryMock = Mock.Get(Mock.Of<IActionEffectFactory>(f =>
				f.CreateMoveEffect(It.IsAny<ActorData>()) == Mock.Of<IActionEffect>()
			));

			IActionFactory actionFactory = new ActionFactory(actionEffectFactoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null);
			IActionResolver playerActionResolver
				= new PlayerActionResolver(null, entityDetector, new InputHolder{Decision = decision}, actionFactory);
			ITileVisibilityUpdater tileVisibilityUpdater = Mock.Of<ITileVisibilityUpdater>();
			var controller = new ActorController(playerActionResolver);

			bool passControl = controller.GiveControl(playerActor, Mock.Of<GameEntity>());

			// assertions:

			passControl.Should().BeTrue();

			// actors' positions should be swapped
			playerActor.LogicalPosition.Should().Be(initialOtherActorPosition);
			otherActor.LogicalPosition.Should().Be(initialPlayerPosition);

			// player energy should equal 0.1 which is initial energy (1) plus gain (0.1) minus action cost (1)
			// toecs bool energyIsCloseToExpected = Mathf.Abs(playerActor.ActorControlData.Energy - 0.1f) < .000001f;
			//Assert.That(energyIsCloseToExpected);

			// player should no longer have fresh field of view since he performed an action
			playerActor.ActorControlData.PreTurnHeartbeatIsDone.Should().BeFalse();

			// correct MoveEffects have been created for both players 
			actionEffectFactoryMock.Verify(f => f.CreateMoveEffect(playerActor));
			actionEffectFactoryMock.Verify(f => f.CreateMoveEffect(otherActor));
		}
	}
*/
}