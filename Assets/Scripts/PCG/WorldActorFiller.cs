namespace PCG
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using FloodSpill;
	using GameLogic.GridRelated;
	using Osnowa;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.CSharpUtilities;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Core.ECS.Initiative;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Example.ECS.AI;
	using Osnowa.Osnowa.Example.ECS.Body;
	using Osnowa.Osnowa.Example.ECS.Creation;
	using Osnowa.Osnowa.Example.ECS.Identification;
	using Osnowa.Osnowa.Example.ECS.View;
	using Osnowa.Osnowa.Rng;
	using UnityEditor;
	using Debug = UnityEngine.Debug;
	using Position = Osnowa.Osnowa.Core.Position;

	class WorldActorFiller : IWorldActorFiller
	{
		private readonly IRandomNumberGenerator _rng;
		private readonly IGameConfig _gameConfig;
		private readonly IEntityGenerator _entityGenerator;
		private readonly GameContext _context;
		private readonly ISavedComponents _savedComponents;
		private readonly IExampleContextManager _contextManager;
		private readonly IFirstPlaceInAreaFinder _placeFinder;

	    public WorldActorFiller(GameContext context, ISavedComponents savedComponents, IRandomNumberGenerator rng,
			IGameConfig gameConfig, IEntityGenerator entityGenerator, IExampleContextManager contextManager,
			IFirstPlaceInAreaFinder placeFinder)
		{
			_rng = rng;
			_gameConfig = gameConfig;
			_entityGenerator = entityGenerator;
			_contextManager = contextManager;

			_context = context;
			_savedComponents = savedComponents;
			_placeFinder = placeFinder;
		}

		public void FillWithActors(float enemyCountRate = 1f)
		{
			IExampleContext context = _contextManager.Current;
			var stopwatch = Stopwatch.StartNew();

			Contexts.sharedInstance.SubscribeId();

			ClearContexts(context);

			MarkIsolatedAreas(context);

			GeneratePlayer(context);
			GenerateWildlife(context, enemyCountRate);

		    //SaveAllComponents();
			List<GameEntity> allEntities = _context.GetEntities().ToList();
			Debug.Log(
				$"Filled the world with {_context.GetEntities().Length} entities in {stopwatch.ElapsedMilliseconds} milliseconds. ");
		}

		private void MarkIsolatedAreas(IOsnowaContext context)
		{
			var floodSpiller = new FloodSpiller();
			PositionFlags positionFlags = context.PositionFlags;
			var stopwatch = Stopwatch.StartNew();

			byte areaIndex = 0;
			byte biggestAreaIndex = 0;
			int maxArea = 0;
			for (int probeX = 0; probeX < context.PositionFlags.XSize; probeX += 10)
			{
				for (int probeY = 0; probeY < context.PositionFlags.YSize; probeY += 10)
				{
					bool isFine = false;
					int totalVisited = 0;
					Position start = new Position(probeX, probeY);
					var parameters = new FloodParameters(start.x, start.y)
					{
						Qualifier =
							(x, y) => positionFlags.IsWalkable(x, y),
                        NeighbourProcessor = (x, y, mark) =>
						{
							if (!isFine)
							{
								isFine = true;
								areaIndex += 1;
							}
							totalVisited += 1;
						}
					};
					int[,] markMatrix = new int[context.PositionFlags.XSize, context.PositionFlags.YSize];
					floodSpiller.SpillFlood(parameters, markMatrix);
					if (totalVisited > 50)
						Debug.Log("visited " + totalVisited + "from " + start.x + ", " + start.y + " with index " + areaIndex);
					if (totalVisited > maxArea)
					{
						maxArea = totalVisited;
						biggestAreaIndex = areaIndex;
					}
				}
			}
			Debug.Log("AFTER: best area index: " + biggestAreaIndex);
			Debug.Log("AFTER: best area: " + maxArea);

			Debug.Log("AFTER: flooding took: " + stopwatch.ElapsedMilliseconds);
		}

		private void GeneratePlayer(IOsnowaContext osnowaContext)
		{
		    Position startingPlayerPosition = new Position(osnowaContext.PositionFlags.XSize / 2, osnowaContext.PositionFlags.YSize / 2); 
            // context.StartingPosition;

			if(osnowaContext.PositionFlags.Get(startingPlayerPosition + Position.Up).HasFlag(PositionFlag.Walkable))
			{
				startingPlayerPosition = startingPlayerPosition + Position.Up;
			}

			if(osnowaContext.PositionFlags.Get(startingPlayerPosition + Position.Up).HasFlag(PositionFlag.Walkable))
			{
				startingPlayerPosition = startingPlayerPosition + Position.Up;
			}

			//Position position = villageWithEntrance?.Entrance ?? context.Villages.First().Square.Positions.First();
			GameEntity playerEntity;
			_entityGenerator.GenerateActorFromRecipeeAndAddToContext(_context, _gameConfig.EntityRecipees.Player,
				startingPlayerPosition, out playerEntity, true);
		}

		private void GenerateWildlife(IOsnowaContext osnowaContext, float enemyCountRate)
		{
			int countPerSpecies = (int) (enemyCountRate * 10);
			GenerateSingleMonsters(osnowaContext, _gameConfig.EntityRecipees.Wolf, countPerSpecies, enemyCountRate);
			GenerateSingleMonsters(osnowaContext, _gameConfig.EntityRecipees.Bear, countPerSpecies, enemyCountRate);
			GenerateSingleMonsters(osnowaContext, _gameConfig.EntityRecipees.Deer, countPerSpecies, enemyCountRate);
		}

		private void GenerateSingleMonsters(IOsnowaContext osnowaContext, IEntityRecipee monsterRecipee, int count, float enemyCountRate)
		{
			for (int i = 0; i < count; i++)
			{
				Position position;

                position = GetRandomWalkablePositionOnBiggestArea(osnowaContext);

				GameEntity singleAnimal;
				_entityGenerator.GenerateActorFromRecipeeAndAddToContext(_context, monsterRecipee, position,
					out singleAnimal);
			}
		}

	    private Position GetRandomWalkablePositionOnBiggestArea(IOsnowaContext osnowaContext, bool farFromPlayer = true, float chanceForPreferredBiomeRequirement = 1f)
		{
            Position position;
			bool positionIsAccepted;
			do
			{
				position = _rng.NextPosition(osnowaContext.PositionFlags.XSize, osnowaContext.PositionFlags.YSize);
				positionIsAccepted = (!farFromPlayer || Position.Distance(osnowaContext.StartingPosition, position) > 25 )
					&&
					osnowaContext.PositionFlags.Get(position).HasFlag(PositionFlag.Walkable);
				
			} while (!positionIsAccepted);
			return position;
		}

		private void ClearContexts(IExampleContext context)
		{
			_context.DestroyAllEntities();
		}

		private void SaveAllComponents()
		{
			throw new NotImplementedException();
			
			_savedComponents.PlayerEntityId = _context.GetPlayerEntity().id.Id;
			_context.RemovePlayerEntity();
			int entitiesCount = _context.GetEntities().Length;

			_savedComponents.Ids = new IdComponent[entitiesCount];
			_savedComponents.Recipees = new RecipeeComponent[entitiesCount];
			_savedComponents.Positions = new PositionComponent[entitiesCount];
			_savedComponents.Visions = new VisionComponent[entitiesCount];
			_savedComponents.Energies = new EnergyComponent[entitiesCount];
			_savedComponents.Integrities = new IntegrityComponent[entitiesCount];
			_savedComponents.Sexes = new SexComponent[entitiesCount];
			_savedComponents.Skills = new SkillsComponent[entitiesCount];
		    _savedComponents.Stomachs = new StomachComponent[entitiesCount];
			_savedComponents.Teams = new TeamComponent[entitiesCount];
			_savedComponents.Looks = new LooksComponent[entitiesCount];

			_savedComponents.EntityToHasComponent = new Dictionary<SGuid, bool[]>();

			for (int index = 0; index < entitiesCount; index++)
			{
				GameEntity entity = _context.GetEntities()[index];
				_savedComponents.EntityToHasComponent[entity.id.Id] = new bool[GameComponentsLookup.TotalComponents];
			}

			for (int index = 0; index < entitiesCount; index++)
			{
				GameEntity entity = _context.GetEntities()[index];
				if (entity.hasId)
				{
					_savedComponents.Ids[index] = entity.id;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Id);
				}
				if (entity.hasPosition)
				{
					_savedComponents.Positions[index] = entity.position;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Position);
				}
				if (entity.hasRecipee)
				{
					_savedComponents.Recipees[index] = entity.recipee;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Recipee);
				}
				if (entity.hasVision)
				{
					_savedComponents.Visions[index] = entity.vision;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Vision);
				}
				if (entity.hasEnergy)
				{
					_savedComponents.Energies[index] = entity.energy;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Energy);
				}
				if (entity.hasIntegrity)
				{
					_savedComponents.Integrities[index] = entity.integrity;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Integrity);
				}
				if (entity.hasSex)
				{
					_savedComponents.Sexes[index] = entity.sex;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Sex);
				}
				if (entity.hasSkills)
				{
					_savedComponents.Skills[index] = entity.skills;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Skills);
				}
				if (entity.hasStomach)
				{
					_savedComponents.Stomachs[index] = entity.stomach;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Stomach);
				}
				if (entity.hasTeam)
				{
					_savedComponents.Teams[index] = entity.team;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Team);
				}
				if (entity.hasLooks)
				{
					_savedComponents.Looks[index] = entity.looks;
					MarkAsComponentAsPresent(entity, GameComponentsLookup.Looks);
				}
			}

			#if UNITY_EDITOR
			EditorUtility.SetDirty(_savedComponents as SavedComponents);
			#endif
		}

		private void MarkAsComponentAsPresent(GameEntity entity, int componentLookupId)
		{
			_savedComponents.EntityToHasComponent[entity.id.Id][componentLookupId] = true;
		}
	}
}