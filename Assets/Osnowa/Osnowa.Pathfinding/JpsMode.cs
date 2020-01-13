namespace Osnowa.Osnowa.Pathfinding
{
	public enum JpsMode
	{
		Normal, // doesn't allow diagonal movement between walls
		AllowDiagonalBetweenWalls, // allows diagonal movement between walls
		DontCutEdges // uses stricter walking, with the goal of not cutting edges
	}
}