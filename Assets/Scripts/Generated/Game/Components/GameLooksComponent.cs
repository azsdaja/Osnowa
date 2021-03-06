//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Osnowa.Osnowa.Example.ECS.View.LooksComponent looks { get { return (Osnowa.Osnowa.Example.ECS.View.LooksComponent)GetComponent(GameComponentsLookup.Looks); } }
    public bool hasLooks { get { return HasComponent(GameComponentsLookup.Looks); } }

    public void AddLooks(UnityEngine.Sprite newBodySprite) {
        var index = GameComponentsLookup.Looks;
        var component = (Osnowa.Osnowa.Example.ECS.View.LooksComponent)CreateComponent(index, typeof(Osnowa.Osnowa.Example.ECS.View.LooksComponent));
        component.BodySprite = newBodySprite;
        AddComponent(index, component);
    }

    public void ReplaceLooks(UnityEngine.Sprite newBodySprite) {
        var index = GameComponentsLookup.Looks;
        var component = (Osnowa.Osnowa.Example.ECS.View.LooksComponent)CreateComponent(index, typeof(Osnowa.Osnowa.Example.ECS.View.LooksComponent));
        component.BodySprite = newBodySprite;
        ReplaceComponent(index, component);
    }

    public void RemoveLooks() {
        RemoveComponent(GameComponentsLookup.Looks);
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

    static Entitas.IMatcher<GameEntity> _matcherLooks;

    public static Entitas.IMatcher<GameEntity> Looks {
        get {
            if (_matcherLooks == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Looks);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLooks = matcher;
            }

            return _matcherLooks;
        }
    }
}
