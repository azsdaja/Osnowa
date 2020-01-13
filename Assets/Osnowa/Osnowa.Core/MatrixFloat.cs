namespace Osnowa.Osnowa.Core
{
    using System;

    [Serializable]
    public class MatrixFloat : MatrixGeneric<float>
    {
        public MatrixFloat(int xSize, int ySize) : base(xSize, ySize)
        {
        }
    }
}