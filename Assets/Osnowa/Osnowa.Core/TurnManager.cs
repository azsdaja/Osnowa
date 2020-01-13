namespace Osnowa.Osnowa.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Context;
    using Entitas;
    using Example;
    using GameLogic.GameCore;
    using global::Osnowa.Osnowa.Example.ECS;

    public class TurnManager : ITurnManager
    {
        private int _lastTurnWhenRemovedDeadActors;
        private int _selectedActorIndex;

        private int _currentActorIndex;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        
        private Queue<GameEntity> _entitiesToHaveTurn = new Queue<GameEntity>(100);

        private readonly IWorldClock _worldClock;
        private GameContext _gameContext;
        private readonly PerInitiativeFeature _perInitiativeFeature;
        private readonly RealTimeFeature _realTimeFeature;
        private IGroup<GameEntity> _energyReadyEntities;
        private IGroup<GameEntity> _entitiesWithEnergy;

        private IOsnowaContextManager _contextManager;

        public TurnManager(IWorldClock worldClock, 
            PerInitiativeFeature perInitiativeFeature, RealTimeFeature realTimeFeature,
            IOsnowaContextManager contextManager)
        {
            _worldClock = worldClock;
            _perInitiativeFeature = perInitiativeFeature;
            _realTimeFeature = realTimeFeature;
            _contextManager = contextManager;
        }

        public void OnGameStart()
        {
            _gameContext = Contexts.sharedInstance.game;
            _energyReadyEntities = _gameContext.GetGroup(GameMatcher.EnergyReady);
            _entitiesWithEnergy = _gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.Energy));

            _stopwatch.Start();
        }

        public void Update()
        {
            _stopwatch.Reset();

            bool needsInput = _gameContext.isWaitingForInput && (_gameContext.playerDecision.Decision == Decision.None);
            if (needsInput)
            {
                //QualitySettings.vSyncCount = 1;
                return;
            }

            _entitiesToHaveTurn.Clear();
            foreach (GameEntity gameEntity in _entitiesWithEnergy)
            {
                if (gameEntity.energy.Energy >= 1f)
                {
                    gameEntity.isEnergyReady = true;
                    gameEntity.isExecutePreTurn = true;
                    gameEntity.isFinishedTurn = false; 
                    _entitiesToHaveTurn.Enqueue(gameEntity);
                }
            }
            
            bool someEntitiesWaitForControl = _entitiesToHaveTurn.Count > 0;
            if (someEntitiesWaitForControl)
            {
                //QualitySettings.vSyncCount = 0;
                while (_entitiesToHaveTurn.Count > 0)
                {
                    GameEntity currentEntity = _entitiesToHaveTurn.Dequeue();
                    // todo tu się wywala jeśli postać została ubita, a dalej jest w kolejce
                    _perInitiativeFeature.Execute();

                    bool shouldFinishFrame = _gameContext.isWaitingForInput || _stopwatch.ElapsedMilliseconds > 30;
                    if (shouldFinishFrame)
                    {
                        break;
                    }
                }

                // an entity can be destroyed while having energy and
                // apparently this may cause infinite loop if we don't do the below:
                // if (_energyReadyEntities.count == 0)
                //     _gameContext.isEnergyReadyEntitiesExistUsun = false;
            }
            else
            {
                GiveEnergyToAllEntities();

                _worldClock.HandleSegment(); // teraz nieregularny
            }
        }

        private void GiveEnergyToAllEntities()
        {
            foreach (GameEntity entity in _entitiesWithEnergy.GetEntities())
            {
                float newEnergy = entity.energy.Energy + entity.energy.EnergyGainPerSegment;
                entity.ReplaceEnergy(entity.energy.EnergyGainPerSegment, newEnergy);
            }
        }
    }
}