namespace Osnowa.Osnowa.Example.ECS.AI
{
    using System.Collections.Generic;
    using Entitas;
    using GameLogic.AI.Model;

    public class StimuliComponent : IComponent
    {
        public List<Stimulus> Stimuli;
    }
}