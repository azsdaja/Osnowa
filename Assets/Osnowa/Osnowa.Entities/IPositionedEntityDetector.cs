namespace Osnowa.Osnowa.Entities
{
    using System.Collections.Generic;
    using Core;

    public interface IPositionedEntityDetector
    {
        IEnumerable<IPositionedEntity> DetectEntities(Position targetPosition, int detectionRange);
        IEnumerable<IPositionedEntity> DetectEntities(Position targetPosition);
    }
}