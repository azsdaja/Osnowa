namespace GameLogic.Entities
{
	public class FriendshipResolver : IFriendshipResolver
	{
		public bool AreFriends(GameEntity entity1, GameEntity entity2)
		{
			// temporary solution - all entites are NOT FRIENDS with currently controlled entity
			return entity1.isPlayerControlled == entity2.isPlayerControlled;
		}
	}
}