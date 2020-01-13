using Osnowa.Osnowa.Example.ECS.Combat;

namespace Osnowa.Osnowa.Example.ECS
{
    public sealed class ReactiveFeature : Feature
    {
        public ReactiveFeature(ReceiveDamageSystem receiveDamageSystem)
        {
            Add(receiveDamageSystem);
        }
    }
}