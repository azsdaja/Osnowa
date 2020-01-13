namespace Osnowa.Osnowa.FOV
{
	using System;
	using System.Collections.Generic;
	using Core;

	public interface IBasicFovPostprocessor
	{
		/// <summary>
		/// In postprocessing, in order to fix some artifacts, we include in the visible set the wall tiles that are behind a visible floor tile.
		/// </summary>
		IEnumerable<Position> PostprocessBasicFov(
			HashSet<Position> visibleBeforePostProcessing, Position centerOfSquareToPostProcess, int rayLength, Func<Position, bool> isWalkable);
	}
}