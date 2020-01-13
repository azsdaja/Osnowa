using System.Collections.Generic;

namespace Osnowa.Osnowa.Core.CSharpUtilities
{
    public static class IEnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> iEnumerable)
        {
            return new HashSet<T>(iEnumerable);
        }
    }
}