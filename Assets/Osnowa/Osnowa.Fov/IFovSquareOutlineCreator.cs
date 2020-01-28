namespace Osnowa.Osnowa.Fov
{
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// A helper for Basic field of view algorithm. See <see cref="BasicFovCalculator"/>.
	/// </summary>
	public interface IFovSquareOutlineCreator
	{
		/// <summary>
		/// Creates a list of points outlining a square around Field of View, so that Bresenham lines can be cast to the outline.
		/// </summary>
		IEnumerable<Position> CreateSquareOutline(Position fovCenter, int sightRange);
	}
}