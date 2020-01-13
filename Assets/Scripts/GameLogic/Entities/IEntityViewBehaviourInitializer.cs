namespace GameLogic.Entities
{
	using Osnowa.Osnowa.Unity;

	public interface IEntityViewBehaviourInitializer
	{
		/// <summary>
		/// Performs any necessary actor behaviour initialization according to his definition.
		/// </summary>
		void Initialize(EntityViewBehaviour entityViewBehaviour);
	}
}