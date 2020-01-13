using System.Collections.Generic;
using System.Linq;
using Osnowa.Osnowa.Grid;
using UnityEngine;

namespace Osnowa.Osnowa.Unity
{
    using Core;
    using Entities;

    public class UnityEntityDetector : IEntityDetector, IPositionedEntityDetector
    {
        private readonly IUnityGridInfoProvider _unityGridInfoProvider;
        public float CellSizeEpsilon => .0125f;
        private readonly int _entityLayerMask;

        public UnityEntityDetector(IUnityGridInfoProvider unityGridInfoProvider)
        {
            _unityGridInfoProvider = unityGridInfoProvider;

            const string entityLayerMask = "Entity";
            _entityLayerMask = 1 << LayerMask.NameToLayer(entityLayerMask);
        }

        public IEnumerable<GameEntity> DetectEntities(Position targetPosition)
        {
            Collider2D[] collidersHit = GetCollidersInRange(targetPosition, _entityLayerMask);
            return FilterEntities(collidersHit);
        }

        public IEnumerable<GameEntity> DetectEntities(Position targetPosition, int detectionRange)
        {
            Collider2D[] collidersHit = GetCollidersInRange(targetPosition, detectionRange, _entityLayerMask);
            return FilterEntities(collidersHit);
        }

        IEnumerable<IPositionedEntity> IPositionedEntityDetector.DetectEntities(Position targetPosition)
        {
            Collider2D[] collidersHit = GetCollidersInRange(targetPosition, _entityLayerMask);
            return FilterPositionedEntities(collidersHit);
        }   

        IEnumerable<IPositionedEntity> IPositionedEntityDetector.DetectEntities(Position targetPosition, int detectionRange)
        {
            Collider2D[] collidersHit = GetCollidersInRange(targetPosition, detectionRange, _entityLayerMask);
            return FilterPositionedEntities(collidersHit);
        }

        private IEnumerable<GameEntity> FilterEntities(Collider2D[] hitObjects)
        {
            IEnumerable<GameEntity> entitiesHit = hitObjects
                .Select(c => c.GetComponent<EntityViewBehaviour>())
                .Select(v => v.Entity)
                .Where(e => e.hasPosition);

            return entitiesHit;
        }

        private IEnumerable<PositionedEntity> FilterPositionedEntities(Collider2D[] hitObjects)
        {
            IEnumerable<PositionedEntity> entitiesHit = hitObjects
                .Select(c => c.GetComponent<EntityViewBehaviour>())
                .Select(b => b.PositionedEntity);

            return entitiesHit;
        }

        private Collider2D[] GetCollidersInRange(Position targetPosition, int cellsRangeInVision, int layerMask)
        {
            Vector2 targetWorldPosition2D = _unityGridInfoProvider.GetCellCenterWorld(targetPosition);
            float toCornerOfArea = cellsRangeInVision*_unityGridInfoProvider.CellSize;
            var bottomLeftCorner = targetWorldPosition2D + new Vector2(-toCornerOfArea, -toCornerOfArea);
            var topRightCorner = targetWorldPosition2D + new Vector2(toCornerOfArea, toCornerOfArea);

            Collider2D[] hitColliders = Physics2D.OverlapAreaAll(bottomLeftCorner, topRightCorner, layerMask);
            return hitColliders;
        }

        private Collider2D[] GetCollidersInRange(Position targetPosition, int layerMask)
        {
            Vector2 targetWorldPosition2D = _unityGridInfoProvider.GetCellCenterWorld(targetPosition);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetWorldPosition2D, CellSizeEpsilon, layerMask);
            return hitColliders;
        }
    }
}