using System;
using Entitas;

namespace Osnowa.Osnowa.Example.ECS.Combat
{
    [Serializable]
    public class ReceiveDamageComponent : IComponent
    {
        public int DamageReceived;
        public Guid DamageSourceEntity;
    }
}