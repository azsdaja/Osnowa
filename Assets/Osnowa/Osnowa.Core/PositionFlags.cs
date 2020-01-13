namespace Osnowa.Osnowa.Core
{
	using System;
	using CSharpUtilities;

	/// <summary>
    /// Stores information about boolean flags assigned to positions. You can use your own ulong-based enums for definining the flags.
    /// 1 is always reserved for walkability and 2 for passing light (visibility).
    /// </summary>
    /// <example>
    /// Example of usage with own flags:
    /// bool hasMyFlag = Get(x, y).HasFlag(MyFlags.SomeValue);
    /// </example>
	[Serializable]
	public class PositionFlags : MatrixGeneric<ulong>
	{
		private readonly ulong _walkableFlagAsUlong;
		private readonly ulong _passingLightFlagAsUlong;

		public PositionFlags(int xSize, int ySize) : base(xSize, ySize)
		{
			_walkableFlagAsUlong = Convert.ToUInt64(PositionFlag.Walkable);
			_passingLightFlagAsUlong = Convert.ToUInt64(PositionFlag.PassingLight);
		}

		public void SetFlags(int x, int y, bool isWalkable, bool isPassingLight)
		{
			PositionFlag flags = PositionFlag.None;
			if(isWalkable) flags |= PositionFlag.Walkable;
			if(isPassingLight) flags |= PositionFlag.PassingLight;
			Set(x, y, (ulong)flags);
		}

		public void SetFlag(int x, int y, ulong flag, bool value)
		{
			ulong currentFlags = Get(x, y);
			ulong flagsWithFlagSet = currentFlags.WithFlagSet(flag, value);
			Set(x, y, flagsWithFlagSet);
		}

	    public bool IsWalkable(int x, int y)
	    {
	        return Get(x, y).HasFlag(_walkableFlagAsUlong);
        }

	    public bool IsWalkable(Position position)
	    {
	        return IsWalkable(position.x, position.y);
	    }

	    public bool IsPassingLight(int x, int y)
	    {
	        return Get(x, y).HasFlag(_passingLightFlagAsUlong);
        }

	    public bool IsPassingLight(Position position)
	    {
	        return IsPassingLight(position.x, position.y);
	    }
    }
}