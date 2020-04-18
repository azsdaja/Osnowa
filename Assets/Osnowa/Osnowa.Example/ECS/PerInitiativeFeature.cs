using Osnowa.Osnowa.Core.ECS;
using Osnowa.Osnowa.Core.ECS.Initiative;
using Osnowa.Osnowa.Core.ECS.Lifetime;
using Osnowa.Osnowa.Example.ECS.Abilities;
using Osnowa.Osnowa.Example.ECS.Body;
using Osnowa.Osnowa.Example.ECS.Heartbeat;
using Osnowa.Osnowa.Example.ECS.Inventory;
using Osnowa.Osnowa.Example.ECS.Statuses;

namespace Osnowa.Osnowa.Example.ECS
{
    using Senses;

    public sealed class PerInitiativeFeature : Feature
    {
        public PerInitiativeFeature(GiveControlSystem giveControlSystem, StimuliSystem stimuliSystem, 
            PreTurnSystem preTurnSystem, PostTurnSystem postTurnSystem,
            PresentSurroundingsSystem presentSurroundingsSystem, PositionStablenessSystem positionStablenessSystem,
            MoveHeldAlongSystem moveHeldAlongSystem, GameEventSystems gameEventFeature,
            AnyStatusChangedSystem statusChangedSystem, ControlledEntityChangedSystem controlledEntityChangedSystem,
            StatusCountdownSystem statusCountdownSystem, LoadViewSystem loadViewSystem,
            DestroyEntitySystem destroyEntitySystem, IntegrityChangedSystem integrityChangedSystem,
            ResolveAbilitiesPerTurnSystem resolveAbilitiesPerTurnSystem,
            PlayerInventoryChangedSystem playerInventoryChangedSystem, DeathClockSystem deathClockSystem, HungerSystem hungerSystem)
        {
            Add(loadViewSystem);

            Add(controlledEntityChangedSystem);
            Add(presentSurroundingsSystem);

            Add(resolveAbilitiesPerTurnSystem);
            Add(hungerSystem);
            Add(stimuliSystem);
            Add(preTurnSystem);

            Add(giveControlSystem);

            Add(positionStablenessSystem);
            Add(moveHeldAlongSystem);
            Add(postTurnSystem);
            Add(deathClockSystem);

            Add(integrityChangedSystem);
            Add(statusCountdownSystem);
            Add(statusChangedSystem);
            Add(playerInventoryChangedSystem);

            Add(destroyEntitySystem);
            Add(gameEventFeature);
        }
    }
}