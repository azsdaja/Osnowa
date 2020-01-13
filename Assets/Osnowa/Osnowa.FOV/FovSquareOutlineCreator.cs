namespace Osnowa.Osnowa.FOV
{
	using System.Collections.Generic;
	using Core;

	public class FovSquareOutlineCreator : IFovSquareOutlineCreator
	{
		public IEnumerable<Position> CreateSquareOutline(Position fovCenter, int sightRange)
		{
			if (sightRange == 0)
			{
				yield return fovCenter;
				yield break;
			}

			for (int xOnOutline = fovCenter.x - sightRange; xOnOutline <= fovCenter.x + sightRange; xOnOutline++)
			{
				int yOnNorthernOutline = fovCenter.y + sightRange;
				yield return new Position(xOnOutline, yOnNorthernOutline);
				int yOnSouthernOutline = fovCenter.y - sightRange;
				yield return new Position(xOnOutline, yOnSouthernOutline);
			}
			for (int yOnOutline = fovCenter.y - sightRange + 1; yOnOutline <= fovCenter.y + sightRange - 1; yOnOutline++)
			{
				int xOnWesternOutline = fovCenter.x - sightRange;
				yield return new Position(xOnWesternOutline, yOnOutline);
				int xOnEasternOutline = fovCenter.x + sightRange;
				yield return new Position(xOnEasternOutline, yOnOutline);
			}
		}
	}
}