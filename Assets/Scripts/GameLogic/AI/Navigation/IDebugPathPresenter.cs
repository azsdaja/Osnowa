namespace GameLogic.AI.Navigation
{
	using System.Collections.Generic;
	using Osnowa.Osnowa.Core;

	public interface IDebugPathPresenter
	{
		void PresentStraight(IList<Position> path);
		void PresentNonStraight(List<Position> path);
	}
}