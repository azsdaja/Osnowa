namespace GameLogic.Entities
{
	public class FriendshipResolver : IFriendshipResolver
	{
		public bool AreFriends(GameEntity source, GameEntity target)
		{
			// temporary solution
			return source.isPlayerControlled == target.isPlayerControlled;
		}
	}
}