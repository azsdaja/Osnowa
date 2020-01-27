namespace IoC
{
	using System;
	using Entitas;
	using FloodSpill;
	using GameLogic;
	using GameLogic.ActionLoop;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core.ActionLoop;
	using Osnowa.Osnowa.Example;
	using Osnowa.Osnowa.Pathfinding.PathPresentationForDebug;
	using Osnowa.Osnowa.Rng;
	using Osnowa.Osnowa.Unity;
	using PCG;
	using UI;
	using UnityEngine;
	using UnityUtilities;
	using Zenject;
	using Random = UnityEngine.Random;
	using SceneContext = SceneContext;

	public class GameGlobalsInstaller : Installer<SceneContext, IGameConfig, IUiManager, ISavedComponents, GameGlobalsInstaller>
	{
					#pragma warning disable IDE0044 // Add readonly modifier
					#pragma warning disable 649
		[Inject]
		private readonly ISceneContext _sceneContext;
		[Inject]
		private readonly IGameConfig _gameConfig;
		[Inject]
		private readonly IUiManager _uiManager;
		[Inject]
		private readonly ISavedComponents _savedComponents;

		private readonly GameContext _context = Contexts.sharedInstance.game; 
					#pragma warning restore IDE0044 // Add readonly modifier
					#pragma warning restore 649

		/// <summary>
		/// Defines how the dependencies between classes will be resolved.
		/// </summary>
		public override void InstallBindings()
		{
			EntityViewBehaviour entityViewPrefab = Resources.Load<EntityViewBehaviour>("Prefabs/Pooled/" + PoolingManager.EntityView);
			AbilityView abilityViewPrefab = Resources.Load<AbilityView>("Prefabs/Pooled/" + PoolingManager.AbilityView);
			Container.BindFactory<EntityViewBehaviour, EntityViewBehaviour.Factory>().FromComponentInNewPrefab(entityViewPrefab).AsSingle();
			Container.BindFactory<AbilityView, AbilityView.Factory>().FromComponentInNewPrefab(abilityViewPrefab).AsSingle();
			
			// creates default bindings for all non-generic interfaces with single implementations.
			Container.Bind(x => x.AllInterfaces()
					.Where(i =>
						i != typeof(ISceneContext)
						&& i != typeof(IGameConfig)
						&& i != typeof(IUiFacade)
						&& i != typeof(IUiManager)
						&& i != typeof(IRandomNumberGenerator)
						&& i != typeof(IActionResolver)
						&& i != typeof(IInputWithRepeating)
						&& i != typeof(IMapGenerationManager)
						&& i != typeof(ISavedComponents)
						&& i != typeof(IDisposable)
						&& i != typeof(IOsnowaContext)
						&& i != typeof(IOsnowaContextManager)
						&& i != typeof(IExampleContextManager)
					)
				)
				.To(x => x.AllNonAbstractClasses()
                    .InNamespaces("Assets.Scripts", "Assets.Osnowa", "Osnowa", "Initialization", "UnityUtilities", "GameLogic", "UI", "AssetTools", "PCG")
                    .Where(
	                    type => // kurde, on tu wchodzi dla wszystkich klas chyba i tworzy AsSingle nawet jeśli nie wiąże, a potem jest problem poniżej
		                    type != typeof(AbilityView.Factory)
		                    && type != typeof(EntityViewBehaviour.Factory)
		                    && type != typeof(ExampleContextManager)
		                    
		                    && type != typeof(ActionResolver)
		                    && type != typeof(AiActionResolver)
		                    && type != typeof(PlayerActionResolver)
		                    
		                    && type != typeof(TilemapGenerator) // dlaczego tego dziada muszę tu wstawić? 
		                    // niżej mam Container.Bind<TilemapGenerator>().ToSelf().AsSingle(); przy FromResolve() się wypieprza
		                    && !typeof(ISystem).IsAssignableFrom(type)
	                    )
				)
				.AsSingle();

			/*Container.Bind(x => x.AllNonAbstractClasses()
				.InNamespaces("Assets.Scripts.ECS", "ECS.Features", "Assets.Osnowa", "Osnowa")
				.Where(type => !type.Name.Contains("Factory"))
				.Where(type => type != typeof(Osnowa.OsnowaUnity.SceneContext))
				).ToSelf().AsSingle();*/
			
			Container.Bind(x => x.AllNonAbstractClasses()
				.InNamespaces("Osnowa")
				.Where(type => typeof(ISystem).IsAssignableFrom(type))
				).ToSelf().AsSingle();
			
			Container.Bind<GameEventSystems>().ToSelf().AsSingle();
			Container.Bind<Contexts>().FromInstance(Contexts.sharedInstance).AsSingle();

			Container.Bind(typeof(IOsnowaContextManager), typeof(IExampleContextManager)).To<ExampleContextManager>().AsSingle();

//		    Container.Bind<IMapGenerationManager>().To<MapGenerationManager>().AsSingle();*/
		    Container.Bind<ISceneContext>().FromInstance(_sceneContext).AsSingle();
			Container.Bind<IGameConfig>().FromInstance(_gameConfig).AsSingle();
			Container.Bind<IUiFacade>().FromInstance(_uiManager).AsSingle();
			Container.Bind<IUiManager>().FromInstance(_uiManager).AsSingle();
			Container.Bind<ISavedComponents>().FromInstance(_savedComponents).AsSingle();
			int rngSeed = _gameConfig.RngSeed == 0 ? Random.Range(0, int.MaxValue) : _gameConfig.RngSeed;
			Container.Bind<IRandomNumberGenerator>().FromInstance(new RandomNumberGenerator(rngSeed)).AsSingle();
			var inputWithRepeating = new InputWithRepeating(initialTimeLeftToRepeat: 0.35f, repeatInterval: 0.06f);
			Container.Bind<IInputWithRepeating>().FromInstance(inputWithRepeating).AsSingle();
			
			Container.Bind(typeof(GameContext), typeof(IContext<GameEntity>)).FromInstance(_context).AsSingle();

			Container.Bind<IActionResolver>().WithId("_actionResolver").To<ActionResolver>().AsSingle();
			Container.Bind<IActionResolver>().WithId("_aiActionResolver").To<AiActionResolver>().AsSingle();
			Container.Bind<IActionResolver>().WithId("_playerActionResolver").To<PlayerActionResolver>().AsSingle();
			
			Container.Bind<TilemapGenerator>().ToSelf().AsSingle();
			
			Container.Bind<FloodSpiller>().AsSingle();
			Container.Bind<FloodScanlineSpiller>().AsSingle();
			
			Container.Bind<Material>().FromInstance(Resources.Load<Material>("Materials/Plain"))
				.WhenInjectedInto<PathRenderer>();
		}
	}
}
