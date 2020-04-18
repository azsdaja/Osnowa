namespace Osnowa.Osnowa.Grid
{
	using Core;

	public interface IVisibilityUpdater
	{
		/// <summary>
		/// Recalculates the field of view and updates visibility of tiles and entities placed on them.
		/// </summary>
		void UpdateVisibility(Position observerPosition, int sightRange, GameEntity entity);
	}
}