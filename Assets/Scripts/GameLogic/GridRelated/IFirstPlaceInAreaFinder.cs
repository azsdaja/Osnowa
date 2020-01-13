namespace GameLogic.GridRelated
{
	using Osnowa.Osnowa.Core;

	public interface IFirstPlaceInAreaFinder
	{
		Position? FindForItem(Position source);
		Position? FindForActor(Position source);
	}
}