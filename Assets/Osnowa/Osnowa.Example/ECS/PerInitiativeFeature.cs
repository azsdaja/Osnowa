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
    public sealed class PerInitiativeFeature : Feature
    {
        public PerInitiativeFeature(GiveControlSystem giveControlSystem,
            PreTurnSystem preTurnSystem, PostHeartbeatSystem postHeartbeatSystem,
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
            Add(preTurnSystem);
            
            Add(giveControlSystem);

            Add(positionStablenessSystem);
            Add(moveHeldAlongSystem);
            Add(postHeartbeatSystem);
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