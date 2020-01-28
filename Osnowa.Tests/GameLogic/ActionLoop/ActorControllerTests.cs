namespace Osnowa.Tests.GameLogic.ActionLoop
{
/*
	[TestFixture]
	class ActorControllerTests
	{
		class TestGameAction : GameAction
		{
			public TestGameAction(SpecificActorData actorData, GameEntity entity, float energyCost) : base(actorData, entity, energyCost, Mock.Of<IActionEffectFactory>())
			{
			}

			public override IEnumerable<IActionEffect> Execute() { return Enumerable.Empty<IActionEffect>(); }
		}

		[Test]
		public void GiveControl_ActorHasNotEnoughEnergy_PassesControlAndDoesNotResolveAction()
		{
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData actorData = new ActorData(0) {ActorControlData = new ActorControlData{ Energy = 0.1f } };

			bool result = actorController.GiveControl(actorData);

			result.Should().BeTrue();
		}

		[Test]
		public void GiveControl_ActorHasNotEnoughEnergy_EnergyIsIncreasedByEnergyGain()
		{
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData actorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = true,
					Energy = 0.1f,
					EnergyGain = 0.2f
				}
			};
			float expectedEnergy = .3f;

			actorController.GiveControl(actorData);

			actorData.ActorControlData.Energy.Should().Be(expectedEnergy);
		}
		
		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionHasNotBeenResolved_ActorHoldsControl()
		{
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData actorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = true,
					Energy = 1
				}
			};

			bool result = actorController.GiveControl(actorData);

			result.Should().BeFalse();
		}
		
		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndDoesNotHaveFreshFieldOfView_FieldOfViewIsRecalculated()
		{
			var tileVisibilityUpdaterMock = new Mock<ITileVisibilityUpdater>();
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData actorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = true,
					Energy = 1,
					PreTurnHeartbeatIsDone = false,
				}
			};

			actorController.GiveControl(actorData);

			tileVisibilityUpdaterMock.Verify(c => c.UpdateTileVisibility(It.IsAny<Position>(), It.IsAny<int>()));
		}
		
		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndPreTurnHeartbeatIsDone_FieldOfViewIsNotRecalculated()
		{
			var tileVisibilityUpdaterMock = new Mock<ITileVisibilityUpdater>();
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData actorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = true,
					Energy = 1,
					PreTurnHeartbeatIsDone = true
				}
			};

			actorController.GiveControl(actorData);

			tileVisibilityUpdaterMock.Verify(c => c.UpdateTileVisibility(It.IsAny<Position>(), It.IsAny<int>()), Times.Never);
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionIsResolved_EnergyIsIncreasedByGain()
		{
			float initialEnergy = 1.0f;
			float energyGain = .1f;
			float actionEnergyCost = .5f;
			float expectedEnergy = .6f;
			var actorData = new ActorData(0) { ActorControlData = new ActorControlData{ ControlledByPlayer = false, Energy = initialEnergy, EnergyGain = energyGain } };
			IActionResolver resolver = Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) 
				== new TestGameAction(actorData, actionEnergyCost));
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			
			actorController.GiveControl(actorData);

			actorData.ActorControlData.Energy.Should().Be(expectedEnergy);
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionIsResolved_FieldOfViewIsMarkedAsNotFresh()
		{
			float initialEnergy = 1.0f;
			float energyGain = .1f;
			float actionEnergyCost = .5f;
			var actorData = new ActorData(0) { ActorControlData = new ActorControlData{ ControlledByPlayer = false, Energy = initialEnergy, EnergyGain = energyGain } };
			IActionResolver resolver = Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) 
				== new TestGameAction(actorData, actionEnergyCost));
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			
			actorController.GiveControl(actorData);

			actorData.ActorControlData.PreTurnHeartbeatIsDone.Should().Be(false);
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionIsResolved_ActionIsExecuted()
		{
			float initialEnergy = 1.0f;
			float energyGain = .1f;
			float actionEnergyCost = .5f;
			var actorData = new ActorData(0) { ActorControlData = new ActorControlData{ ControlledByPlayer = false, Energy = initialEnergy, EnergyGain = energyGain } };
			var actionToExecuteMock = new Mock<GameAction>(actorData, actionEnergyCost, Mock.Of<IActionEffectFactory>());
			IActionResolver resolver = Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) 
				== actionToExecuteMock.Object);
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			
			actorController.GiveControl(actorData);

			actionToExecuteMock.Verify(e => e.Execute());
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionIsResolved_ActionEffectIsProcessed()
		{
			float initialEnergy = 1.0f;
			float energyGain = .1f;
			var actorData = new ActorData(0) { ActorControlData = new ActorControlData{ ControlledByPlayer = false, Energy = initialEnergy, EnergyGain = energyGain } };
			var effectToProcessMock = new Mock<IActionEffect>();
			var actionToExecute = Mock.Of<IGameAction>(a => a.Execute() == new[]{effectToProcessMock.Object});
			IActionResolver resolver = Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) 
				== actionToExecute);
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			
			actorController.GiveControl(actorData);

			effectToProcessMock.Verify(p => p.Process());
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionIsResolved_PassesControl()
		{
			IActionResolver resolver 
				= Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) == new TestGameAction(null, 0f));
			ActorData actorData = new ActorData(0) {ActorControlData = new ActorControlData{ Energy = 1 } };
			var actorController = new ActorController(Mock.Of<IActionResolver>());

			bool result = actorController.GiveControl(actorData);

			result.Should().BeTrue();
		}

		[Test]
		public void GiveControl_ActorHasEnoughEnergyAndActionHasNotBeenResolved_EnergyIsNotIncreasedByGain()
		{
			float initialEnergy = 1.0f;
			float energyGain = .1f;
			float expectedEnergy = initialEnergy;
			ActorData actorData = new ActorData(0) {ActorControlData = new ActorControlData{ Energy = initialEnergy, EnergyGain = energyGain } };
			IActionResolver resolver = Mock.Of<IActionResolver>(r => r.GetAction(It.IsAny<ActorData>()) == null);
			var actorController = new ActorController(Mock.Of<IActionResolver>());

			actorController.GiveControl(actorData);

			actorData.ActorControlData.Energy.Should().Be(expectedEnergy);
		}

		[Test]
		public void GiveControl_AiShouldBeUsed_AiActionResolverIsUsed()
		{
			var aiActionResolver = new Mock<IActionResolver>();
			var playerActionResolver = new Mock<IActionResolver>();
			IActionResolver resolver = new ActionResolver(aiActionResolver.Object, playerActionResolver.Object);
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			var aiActorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = false,
					Energy = 1f
				}
			};

			actorController.GiveControl(aiActorData);

			aiActionResolver.Verify(r => r.GetAction(It.IsAny<ActorData>()));
			playerActionResolver.Verify(r => r.GetAction(It.IsAny<ActorData>()), Times.Never);
		}

		[Test]
		public void GiveControl_AiShouldNotBeUsed_PlayerActionResolverIsUsed()
		{
			var aiActionResolver = new Mock<IActionResolver>();
			var playerActionResolver = new Mock<IActionResolver>();
			IActionResolver resolver = new ActionResolver(aiActionResolver.Object, playerActionResolver.Object);
			var actorController = new ActorController(Mock.Of<IActionResolver>());
			ActorData playerActorData = new ActorData(0)
			{
				ActorControlData = new ActorControlData
				{
					ControlledByPlayer = true,
					Energy = 1
				}
			};

			actorController.GiveControl(playerActorData);

			aiActionResolver.Verify(r => r.GetAction(It.IsAny<ActorData>()), Times.Never);
			playerActionResolver.Verify(r => r.GetAction(It.IsAny<ActorData>()));
		}
	}
*/
}
