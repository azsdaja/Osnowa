namespace UnityUtilities
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public static class Vector3IntExtensions
	{
		public static Position ToPosition(this Vector3Int vector)
		{
			return new Position(vector.x, vector.y);
		}
	}
}
