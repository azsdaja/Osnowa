namespace Osnowa
{
	using Osnowa.Core;

	public interface IFloodArea
	{
		Bounds Bounds { get; }
		int GetValueAtPosition(Position position);
		Position Center { get; }
		int ArraySize { get; }
		Position FurthestPosition { get; }
	}
}