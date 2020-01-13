namespace GameLogic.Entities
{
	public interface IFriendshipResolver
	{
		bool AreFriends(GameEntity source, GameEntity target);
	}
}