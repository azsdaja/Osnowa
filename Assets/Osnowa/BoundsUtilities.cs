namespace Osnowa
{
    using FloodSpill;
    using Osnowa.Core;

    public class BoundsUtilities
    {
        public static FloodBounds ToFloodBounds(Bounds bounds)
        {
            return new FloodBounds(bounds.Min.x, bounds.Min.y, bounds.Size.x, bounds.Size.y);
        }
    }
}