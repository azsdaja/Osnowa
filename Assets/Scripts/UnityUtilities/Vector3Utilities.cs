namespace UnityUtilities
{
	using Osnowa.Osnowa.Core;
	using UnityEngine;

	public class Vector3Utilities
	{
		public static Vector3 LerpThereAndBack(Vector3 a, Vector3 b, float t)
		{
			t = Mathf.Clamp01(t);
			t = -Mathf.Abs(2 * (t - 0.5f)) + 1; // linearly increases from 0 to 1 in range 0-0.5 and then decreases from 1 to 0 in range 0.5-1
			return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		public static Position ToPosition(Vector3 position)
		{
			return new Position(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
		}
	}
}
