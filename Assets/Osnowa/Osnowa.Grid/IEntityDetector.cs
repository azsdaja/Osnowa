using System.Collections.Generic;

namespace Osnowa.Osnowa.Grid
{
	using Core;

	public interface IEntityDetector
	{
		IEnumerable<GameEntity> DetectEntities(Position targetPosition, int detectionRange);
		IEnumerable<GameEntity> DetectEntities(Position targetPosition);
	}
}