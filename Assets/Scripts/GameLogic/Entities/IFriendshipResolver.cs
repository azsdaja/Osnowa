namespace GameLogic.Entities
{
	public interface IFriendshipResolver
	{
		bool AreFriends(GameEntity entity1, GameEntity entity2);
	}
}