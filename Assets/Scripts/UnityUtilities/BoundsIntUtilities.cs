namespace UnityUtilities
{
	using FloodSpill;
	using UnityEngine;
	using Position = Osnowa.Osnowa.Core.Position;

	public class BoundsIntUtilities
	{
		/// <summary>
		/// Returns source BoundsInt which are stretched so that they contain consideredPosition. Ignores z component of bounds.
		/// </summary>
		public static BoundsInt With(BoundsInt source, Position consideredPosition)
		{
			if (consideredPosition.x < source.xMin)
			{
				source.xMin = consideredPosition.x;
			}
			if (consideredPosition.y < source.yMin)
			{
				source.yMin = consideredPosition.y;
			}
			if (consideredPosition.x >= source.xMax)
			{
				source.xMax = consideredPosition.x + 1;
			}
			if (consideredPosition.y >= source.yMax)
			{
				source.yMax = consideredPosition.y + 1;
			}
			return source;
		}

		/// <summary>
		/// Returns source BoundsInt which are stretched so that they contain consideredPosition.
		/// </summary>
		public static BoundsInt With(BoundsInt source, Vector3Int consideredPosition)
		{
			if (consideredPosition.x < source.xMin)
			{
				source.xMin = consideredPosition.x;
			}
			if (consideredPosition.y < source.yMin)
			{
				source.yMin = consideredPosition.y;
			}
			if (consideredPosition.z < source.zMin)
			{
				source.zMin = consideredPosition.z;
			}
			if (consideredPosition.x >= source.xMax)
			{
				source.xMax = consideredPosition.x + 1;
			}
			if (consideredPosition.y >= source.yMax)
			{
				source.yMax = consideredPosition.y + 1;
			}
			if (consideredPosition.z >= source.zMax)
			{
				source.zMax = consideredPosition.z + 1;
			}
			return source;
		}

		public static BoundsInt Zero => new BoundsInt(0,0,0,0,0,0);

		public static BoundsInt CenteredOn(Position center, int range)
		{
			int size = 2*range + 1;
			return new BoundsInt(center.x - range, center.y - range, 0, 
								 size, size, 1);
		}

		public static FloodBounds ToFloodBounds(BoundsInt boundsInt)
		{
			return new FloodBounds(boundsInt.xMin, boundsInt.yMin, boundsInt.size.x, boundsInt.size.y);
		}
	}
}