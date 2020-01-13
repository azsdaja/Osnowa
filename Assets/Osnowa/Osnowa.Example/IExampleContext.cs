namespace Osnowa.Osnowa.Example
{
    using System;
    using Context;

    public interface IExampleContext : IOsnowaContext
    {
		float SeaLevel { get; set; }
        DateTime InGameDate { get; set; }
    }
}