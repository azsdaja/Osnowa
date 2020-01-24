namespace Osnowa.Osnowa.Fov
{
	using System;
	using System.Collections.Generic;
	using Core;

	/// <summary>
	/// Creates a raster line made of points connecting two points on a grid. Stops on blocking cells.
	/// </summary>
	/// <param name="maxLength">Limit for length of line. If set to -1, unlimited.</param>
	/// <example>
	/// .......**2
	/// ...****...
	/// 1**.......
	/// </example>
	public interface IRasterLineCreator
	{
		/// <summary>
		/// Creates a raster line from point 1 to point 2.
		/// </summary>
		IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2);

		/// <summary>
		/// Creates a raster line from point 1 towards point 2, limited by maxLength.
		/// </summary>
		IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, int maxLength);

		/// <summary>
		/// Creates a raster line from point 1 towards point 2, terminated by any blocking point. 
		/// The blocking point can be included in the result or not.
		/// </summary>
		IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, Func<Position, bool> isPassing, bool includeBlockingPoint = false);

		/// <summary>
		/// Works like GetRasterLine, but chooses secondary options for next pixel if the primary option is blocked.
		/// <example>
		/// ...*2
		/// .**#. because of the wall (#) the third star is not placed at primary option (southwest of 2), 
		/// 1.... but can be placed at secondary option (west of 2).
		/// </example>
		/// </summary>
		IList<Position> GetRasterLinePermissive(int currentX, int y1, int x2, int y2, 
			Func<Position, bool> isPassing, int maxLength = -1, bool includeBlockingPoint = false);

		IList<Position> GetRasterLine(int currentX, int y1, int x2, int y2, Func<Position, bool> isPassing,
			int maxLength = -1, bool includeBlockingPoint = true);
	}
}