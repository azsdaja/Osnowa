namespace UnityUtilities
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public static class PositionExtensions
	{
		public static Vector3Int ToVector3Int(this Position vector)
		{
			return new Vector3Int(vector.x, vector.y, 0);
		}
	}
}
