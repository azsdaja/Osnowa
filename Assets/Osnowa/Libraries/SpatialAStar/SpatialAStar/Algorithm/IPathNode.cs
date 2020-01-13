namespace Libraries.SpatialAStar.SpatialAStar.Algorithm
{
	public interface IPathNode<TUserContext>
	{
		float Cost { get; }

		bool IsWalkable(TUserContext inContext);
	}
}