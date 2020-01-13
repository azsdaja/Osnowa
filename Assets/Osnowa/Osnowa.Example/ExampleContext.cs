namespace Osnowa.Osnowa.Example
{
    using System;
    using Context;

    [Serializable]
    public class ExampleContext : OsnowaContext, IExampleContext
    {
        private DateTime _inGameDate;
        private string _inGameDateAsString;
        public float SeaLevel { get; set; }

        public DateTime InGameDate
        {
            get { return _inGameDate; }
            set
            {
                _inGameDate = value;
                _inGameDateAsString = value.ToString("s");

            }
        }

        public ExampleContext(int xSize, int ySize) : base(xSize, ySize)
        {
        }
    }
}