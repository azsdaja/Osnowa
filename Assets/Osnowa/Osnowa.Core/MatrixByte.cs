namespace Osnowa.Osnowa.Core
{
	using System;

	[Serializable]
	public class MatrixByte : MatrixGeneric<byte>
	{
		public MatrixByte(int xSize, int ySize) : base(xSize, ySize)
		{
		}
	}
}