namespace Osnowa.Osnowa.Grid
{
    using Core;

    /// <summary>
    /// Allows easy access to data from current IOsnowaContext related to the game grid.
    /// </summary>
    public interface IGrid
    {
        int XSize { get; }
        int YSize { get; }
        Position MinPosition { get; }
        bool IsWalkable(Position position);
        bool IsWalkableChecked(Position position);
        bool IsPassingLight(Position position);
        bool IsPassingLightChecked(Position position);
        float GetWalkability(Position position);
        void InitializePathfindingData(bool initializeJps);
    }
}