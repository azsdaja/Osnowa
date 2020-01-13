namespace Osnowa.Osnowa.Core
{
	using System;

	[Serializable]
	[Flags]
	public enum PositionFlag : ulong
	{
		None = 0,
		Walkable = 1,
		PassingLight = 2,
	}
}