using Osnowa.Osnowa.Context;

namespace Osnowa.Osnowa.Grid
{
    using System;
    using Core;
    using Core.CSharpUtilities;
    using Pathfinding;

    /// <summary>
    /// Can be used to access PositionFlags efficiently without need to reference context each time.
    /// </summary>
    public class Grid : IGrid
    {
        public int XSize => _positionFlags.XSize;
        public int YSize => _positionFlags.YSize;
        public Position MinPosition { get; }
        
        private PathfindingDataHolder _pathfindingDataHolder;
        private PositionFlags _positionFlags;
        private MatrixFloat _walkability;
        private readonly ulong _passingLightFlagAsUlong;

        public Grid(IOsnowaContextManager contextManager, bool contextShouldBePresent = false)
        {
            if (!contextManager.HasContext)
            {
                if(contextShouldBePresent) throw new ArgumentException("Missing context where it's expeteced to be present");
            }
            else
            {
                _positionFlags = contextManager.Current.PositionFlags;
                _walkability = contextManager.Current.Walkability;
                _pathfindingDataHolder = contextManager.Current.PathfindingData;
            }

            contextManager.ContextReplaced += newContext =>
            {
                _positionFlags = newContext.PositionFlags;
                _walkability = newContext.Walkability;
                _pathfindingDataHolder = newContext.PathfindingData;
            };
            MinPosition = Position.Zero;
            
            _passingLightFlagAsUlong = Convert.ToUInt64(PositionFlag.PassingLight);
        }

        public bool IsWalkable(Position position)
        {
            return _positionFlags.IsWalkable(position);
        }

        public bool IsWalkableChecked(Position position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _positionFlags.XSize || position.y >= _positionFlags.YSize)
            {
                return false;
            }
            return _positionFlags.IsWalkable(position.x, position.y);
        }

        public bool IsPassingLight(Position position)
        {
            return _positionFlags.Get(position.x, position.y).HasFlag(_passingLightFlagAsUlong);
        }

        public bool IsPassingLightChecked(Position position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _positionFlags.XSize || position.y >= _positionFlags.YSize)
            {
                return false;
            }
            return _positionFlags.Get(position.x, position.y).HasFlag(_passingLightFlagAsUlong);
        }

        /// <inheritdoc />
        public float GetWalkability(Position position)
        {
            return _walkability.Get(position);
        }

        public void InitializePathfindingData(bool initializeJps)
        {
            for (int currentX = 0; currentX < XSize; currentX++)
            {
                for (int currentY = 0; currentY < YSize; currentY++)
                {
                    // todo mixing zero-based and non-zero-based concepts here!
                    Position positionToCheckForWall = _pathfindingDataHolder.PositionToNonZeroBasedPosition(new Position(currentX, currentY));
                    bool isWalkable = IsWalkable(positionToCheckForWall);
                    _pathfindingDataHolder.UpdateWalkability(positionToCheckForWall, isWalkable);
                }
            }
        }
        
        public void SetWalkability(Position position, bool isWalkable)
        {
            float walkability = isWalkable ? 1f : 0f;
            
            _walkability.Set(position, walkability);
            _positionFlags.Set(position, 1);
            _pathfindingDataHolder.UpdateWalkability(position, walkability);
        }

        public void SetWalkability(Position position, float walkability)
        {
            _walkability.Set(position, walkability);
            _pathfindingDataHolder.UpdateWalkability(position, walkability);

        }
    }
}