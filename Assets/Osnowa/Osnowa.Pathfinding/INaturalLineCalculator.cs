namespace Osnowa.Osnowa.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using Core;

	/// <summary>Calculates the longest “natural” (straight and raster) line which a character 
	/// can walk to optimally follow the path defined by given jump points.</summary>
	/// <example>
	/// For:<br/>
	/// __2******3<br/>
	/// _*__######<br/>
	/// 1_________<br/>
	/// 		  <br/>
	/// Gives:	  <br/>
	/// ____**____<br/>
	/// __**######<br/>
	/// **________
	/// </example>
	public interface INaturalLineCalculator
	{
		IList<Position> GetFirstLongestNaturalLine(IList<Position> jumpPoints, Func<Position, bool> isWalkable);
		IList<Position> GetFirstLongestNaturalLine(Position startNode, IList<Position> followingJumpPoints, Func<Position, bool> isWalkable);

		/// <summary>
		/// Usually the current JPS implementation creates too many jump points (many of them are aligned in one line).
		/// This function gives three first „natural” jump points (two or three), which means they don't form a single line.
		/// </summary>
		IList<Position> GetNaturalJumpPoints(IList<Position> jumpPoints);
	}
}