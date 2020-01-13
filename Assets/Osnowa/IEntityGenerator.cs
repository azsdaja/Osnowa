using Entitas;

namespace Osnowa
{
	using Osnowa.Core;

	public interface IEntityGenerator
	{
		void GenerateActorFromRecipeeAndAddToContext(IContext<GameEntity> context, IEntityRecipee entityRecipee, Position position, out GameEntity entity,
			bool controlledByPlayer = false);
	}
}