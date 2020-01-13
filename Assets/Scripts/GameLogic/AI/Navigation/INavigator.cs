namespace GameLogic.AI.Navigation
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;

	/// <summary>
	/// Takes care of finding paths and resolving the steps to follow them.
	/// </summary>
	public interface INavigator
	{
		NavigationData GetNavigationData(Position startPosition, Position targetPosition);
		NavigationResult ResolveNextStep(NavigationData navigationData, Position currentPosition, out Position nextStep);

		List<Position> GetJumpPoints(Position startPosition, Position targetPosition);
		List<Position> GetStraightPath(Position startPosition, Position targetPosition);
	}
}