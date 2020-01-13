namespace Osnowa.Osnowa.Core.CSharpUtilities
{
	using System;
	using System.Collections.Generic;

	public class FunctionalComparer<T> : IComparer<T>
	{
		private readonly Func<T, T, int> _comparer;

		public FunctionalComparer(Func<T, T, int> comparer)
		{
			_comparer = comparer;
		}
		public static IComparer<T> Create(Func<T, T, int> comparer)
		{
			return new FunctionalComparer<T>(comparer);
		}
		public int Compare(T x, T y)
		{
			return _comparer(x, y);
		}
	}
}