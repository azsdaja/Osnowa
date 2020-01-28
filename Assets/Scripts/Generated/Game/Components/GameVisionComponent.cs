//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Osnowa.Osnowa.Core.ECS.VisionComponent vision { get { return (Osnowa.Osnowa.Core.ECS.VisionComponent)GetComponent(GameComponentsLookup.Vision); } }
    public bool hasVision { get { return HasComponent(GameComponentsLookup.Vision); } }

    public void AddVision(int newVisionRange, int newPerceptionRange, System.Collections.Generic.HashSet<System.Guid> newEntitiesNoticed) {
        var index = GameComponentsLookup.Vision;
        var component = (Osnowa.Osnowa.Core.ECS.VisionComponent)CreateComponent(index, typeof(Osnowa.Osnowa.Core.ECS.VisionComponent));
        component.VisionRange = newVisionRange;
        component.PerceptionRange = newPerceptionRange;
        component.EntitiesNoticed = newEntitiesNoticed;
        AddComponent(index, component);
    }

    public void ReplaceVision(int newVisionRange, int newPerceptionRange, System.Collections.Generic.HashSet<System.Guid> newEntitiesNoticed) {
        var index = GameComponentsLookup.Vision;
        var component = (Osnowa.Osnowa.Core.ECS.VisionComponent)CreateComponent(index, typeof(Osnowa.Osnowa.Core.ECS.VisionComponent));
        component.VisionRange = newVisionRange;
        component.PerceptionRange = newPerceptionRange;
        component.EntitiesNoticed = newEntitiesNoticed;
        ReplaceComponent(index, component);
    }

    public void RemoveVision() {
        RemoveComponent(GameComponentsLookup.Vision);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherVision;

    public static Entitas.IMatcher<GameEntity> Vision {
        get {
            if (_matcherVision == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Vision);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherVision = matcher;
            }

            return _matcherVision;
        }
    }
}
