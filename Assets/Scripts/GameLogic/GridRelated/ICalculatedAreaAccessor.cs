namespace GameLogic.GridRelated
{
	using Osnowa;
	using Osnowa.Osnowa.Core;

	public interface ICalculatedAreaAccessor
	{
		IFloodArea FetchWalkableFlood(Position center, int floodRange);
		FovArea FetchVisibilityFov(Position center, int sightRange);
	}
}