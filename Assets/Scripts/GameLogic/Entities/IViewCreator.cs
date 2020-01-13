namespace GameLogic.Entities
{
	using Osnowa.Osnowa.Core;

	public interface IViewCreator
	{
		/// <summary>
		/// Creates a GameObject for a new entity and initializes it with components according to its recipee.
		/// </summary>
		IViewController SpawnEntity(IEntityRecipee entityRecipee, Position position, bool forceAggressive = true);

		IViewController InitializeViewForEntity(GameEntity entity);
	}
}