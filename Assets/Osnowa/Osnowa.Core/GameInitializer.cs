namespace Osnowa.Osnowa.Core
{
	using System;
	using System.Collections.Generic;
	using GameLogic.AI;
	using GameLogic.Entities;
	using GameLogic.GameCore;
	using global::Osnowa.Osnowa.Context;
	using global::Osnowa.Osnowa.Core.ECS;
	using global::Osnowa.Osnowa.Core.ECS.Initiative;
	using global::Osnowa.Osnowa.Example.ECS;
	using global::Osnowa.Osnowa.Example.ECS.AI;
	using global::Osnowa.Osnowa.Example.ECS.Body;
	using global::Osnowa.Osnowa.Example.ECS.Creation;
	using global::Osnowa.Osnowa.Example.ECS.Identification;
	using global::Osnowa.Osnowa.Example.ECS.View;
	using global::Osnowa.Osnowa.Pathfinding;
	using Grid;
	using Unity;
	using UnityEngine;
	using UnityUtilities;
	using Zenject;

	// todo póki co robi silnikową inicjalizację, ale zakomentowane elementy odpowiadają dodatkom (przykładowi). rozteguj na Unity i nieunity
	/// <summary>
	/// Performs any necessary initialization when the game is starting.
	/// </summary>
	public class GameInitializer : MonoBehaviour
	{
		[SerializeField] private PoolingManager _poolingManager;
		[SerializeField] private TurnManagerBehaviour _turnManager;
		private bool _gameInitialized;

		
		private ITilemapInitializer _tilemapInitializer;
		private IPathfinder _pathfinder;
		private IGameConfig _gameConfig;
		private IStimulusReceiver _stimulusReceiver;
		private ISavedComponents _savedComponents;
		private GameContext _context;

		private IPositionFlagsResolver _positionFlagsResolver;
		private IGrid _grid;
		private PerInitiativeFeature _perInitiativeFeature;
		private RealTimeFeature _realTimeFeature;
		private IOsnowaContextManager _contextManager;

		[Inject]
		public void Init(IPathfinder pathfinder, ITilemapInitializer tilemapInitializer,
			IEntityViewBehaviourInitializer entityViewBehaviourInitializer, IGameConfig gameConfig, IViewCreator viewCreator,
			IPositionFlagsResolver positionFlagsResolver, IStimulusReceiver stimulusReceiver,
			ISavedComponents savedComponents, GameContext context, PerInitiativeFeature perInitiativeFeature, RealTimeFeature realTimeFeature,
			IOsnowaContextManager contextManager, IGrid grid)
		{
			_realTimeFeature = realTimeFeature;
			_perInitiativeFeature = perInitiativeFeature;
			
			_tilemapInitializer = tilemapInitializer;
			_pathfinder = pathfinder;
			_gameConfig = gameConfig;
			_positionFlagsResolver = positionFlagsResolver;
			_stimulusReceiver = stimulusReceiver;
			_savedComponents = savedComponents;
			_context = context;
			_contextManager = contextManager;
			_grid = grid;
		}

		void Update ()
		{
			if (_gameInitialized)
			{
				return;
			}

			if (!_poolingManager.Initialized)
			{
				_poolingManager.Initialize();
				return;
			}

			if (!_turnManager.gameObject.activeInHierarchy)
			{
				_turnManager.gameObject.SetActive(true);
				return;
			}

			InitializeGame();

			_gameInitialized = true;
		}

		private void InitializeGame()
		{
			_tilemapInitializer.InitializeVisibilityOfTiles(_gameConfig.ModeConfig.Vision);
			_positionFlagsResolver.InitializePositionFlags();
			_grid.InitializePathfindingData(true);

			//ResetEntitasContext();
			//LoadEntities();

			_context.ReplacePlayerDecision(Decision.None, Position.Zero, Position.MinValue);

//			_contextManager.Current.InGameDate = new DateTime(1318, 5, 1, 12, 0, 0);
//			_contextManager.Current.TimeOfDay = TimeOfDay.Day;

			_turnManager.gameObject.SetActive(true);
		}

		/// <summary>
		/// Reset the context of ECS, so that it can be used again. DOESN'T WORK AT THE MOMENT
		/// </summary>
		public void ResetEntitasContext()
		{
			_perInitiativeFeature.DeactivateReactiveSystems();
			_realTimeFeature.DeactivateReactiveSystems();

			_context.DestroyAllEntities();

			_perInitiativeFeature.ActivateReactiveSystems();
			_realTimeFeature.ActivateReactiveSystems();
			Contexts.sharedInstance.SubscribeId();
			_context.ReplacePlayerDecision(Decision.None, Position.Zero, Position.MinValue);
		}

		private void LoadEntities()
		{
			throw new NotImplementedException();
			int entitesCount = _savedComponents.Positions.Length;
			int? firstEntityIndex = null;

			_context.ReplacePlayerEntity(_savedComponents.PlayerEntityId);
			for (int i = 0; i < entitesCount; i++)
			{
				GameEntity entity = _context.CreateEntity();
				if (!firstEntityIndex.HasValue) firstEntityIndex = entity.creationIndex;
				int currentEntityIndex = entity.creationIndex;
				int componentIndex = currentEntityIndex - firstEntityIndex.Value;

				IdComponent savedId = _savedComponents.Ids[componentIndex];
				entity.ReplaceId(savedId.Id);

				bool[] componentPresence = _savedComponents.EntityToHasComponent[savedId.Id];
					
				PositionComponent savedPosition = _savedComponents.Positions[componentIndex];
				if (componentPresence[GameComponentsLookup.Position])
				{
					entity.AddPosition(savedPosition.Position);
					entity.AddPositionAfterLastTurn(default(Position));
				}

				VisionComponent savedVision = _savedComponents.Visions[componentIndex];
				if (componentPresence[GameComponentsLookup.Vision]) entity.AddVision(savedVision.VisionRange, savedVision.PerceptionRange, 
					savedVision.EntitiesNoticed ?? new HashSet<Guid>());

				RecipeeComponent savedRecipee = _savedComponents.Recipees[componentIndex];
				if (componentPresence[GameComponentsLookup.Recipee])
					entity.AddRecipee(savedRecipee.RecipeeName);

				EnergyComponent savedEnergy = _savedComponents.Energies[componentIndex];
				if (componentPresence[GameComponentsLookup.Energy])
					entity.AddEnergy(savedEnergy.EnergyGainPerSegment, savedEnergy.Energy);

				IntegrityComponent savedIntegrity = _savedComponents.Integrities[componentIndex];
				if (componentPresence[GameComponentsLookup.Integrity])
					entity.AddIntegrity(savedIntegrity.Integrity, savedIntegrity.MaxIntegrity);

				SkillsComponent savedSkill = _savedComponents.Skills[componentIndex];
				if (componentPresence[GameComponentsLookup.Skills])
					entity.AddSkills(savedSkill.Skills);

				StomachComponent savedStomach = _savedComponents.Stomachs[componentIndex];
				if (componentPresence[GameComponentsLookup.Stomach])
					entity.AddStomach(savedStomach.Satiation, savedStomach.MaxSatiation);

				TeamComponent savedTeam = _savedComponents.Teams[componentIndex];
				if (componentPresence[GameComponentsLookup.Team])
					entity.AddTeam(savedTeam.Team);

				LooksComponent savedLooks = _savedComponents.Looks[componentIndex];
				if (componentPresence[GameComponentsLookup.Looks])
					entity.AddLooks(savedLooks.BodySprite);

				EdibleComponent savedEdible = _savedComponents.Edibles[componentIndex];
				if (componentPresence[GameComponentsLookup.Edible])
					entity.AddEdible(savedEdible.Satiety);

				if (componentPresence[GameComponentsLookup.BlockingPosition])
					entity.isBlockingPosition = true;

				entity.isFinishedTurn = true;
				if (_context.playerEntity.Id == entity.id.Id)
				{
					entity.isPlayerControlled = true;
				}
			}
		}
	}
}
