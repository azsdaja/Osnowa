namespace GameLogic.AI.Model
{
    using System;
    using Osnowa.Osnowa.Core;

    public struct Stimulus
    {
        public StimulusType Type;
        public Guid ObjectEntityId;
        public Position? Position;
    }
}