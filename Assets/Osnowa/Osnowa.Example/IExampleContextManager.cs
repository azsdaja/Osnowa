namespace Osnowa.Osnowa.Example
{
    using Context;

    public interface IExampleContextManager : IOsnowaContextManager
    {
        new IExampleContext Current { get; }
    }
}