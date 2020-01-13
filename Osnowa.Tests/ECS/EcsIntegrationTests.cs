using System;
using System.Diagnostics;
using Assets.Osnowa.Osnowa.Core;
using Entitas;
using NUnit.Framework;

namespace Osnowa.Tests.ECS
{
	[TestFixture]
	public class EcsIntegrationTests : ZenjectUnitTestFixture
	{
		[Test]
		public void Showcase()
		{
			var stopwatch = Stopwatch.StartNew();
			
			var gameContext = new GameContext();
			int entitiesCount = 10;
			int segments = 10;

			var random = new Random();
			for (int i = 0; i < entitiesCount; i++)
			{
				GameEntity entity = gameContext.CreateEntity();
				entity.AddEnergy(1.0f, (float)random.NextDouble());
			}
			
			
			var turnManager = new TurnManager();

			var perSegmentSystems = new Systems()
//				.Add(new IncreaseEnergySystem(gameContext))
				;

			var perInitiativeSystems = new Systems()
				//.Add(new InitiativeiniGivingSystem(gameContext))
				//.Add(new PostTurnSystem(gameContext)) 
				//.Add(new PreTurnSystem(gameContext)) 
				;

			for (int segment = 0; segment < segments; segment++)
			{
				perSegmentSystems.Execute();
				//while (gameContext.isEnergyReadyEntitiesExist)
				{
					perInitiativeSystems.Execute();
				}
			}

			Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
		}

		[Test]
		public void LoadTest()
		{
			var gameContext = new GameContext();
			int entitiesCount = 1000;
			int segments = 100;

			var random = new Random();
			for (int i = 0; i < entitiesCount; i++)
			{
				GameEntity entity = gameContext.CreateEntity();
				entity.AddEnergy(0.2f, (float)random.NextDouble());
			}

			var systems = new Systems()
				//.Add(new IncreaseEnergySystem(gameContext))
				//.Add(new PreTurnSystem(Contexts.sharedInstance.game))
				//.Add(new InitiativeGivingSystem(Contexts.sharedInstance.game))
				//.Add(new PostTurnSystem(Contexts.sharedInstance.game))
				;

			var stopwatch = Stopwatch.StartNew();

			for (int segment = 0; segment < segments; segment++)
			{
				systems.Execute();
			}

			Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
		}
	}
}