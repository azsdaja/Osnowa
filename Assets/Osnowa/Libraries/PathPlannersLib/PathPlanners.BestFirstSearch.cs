namespace Libraries.PathPlannersLib
{
    public abstract class BestFirstSearch : SearchAlgorithm
    {
        protected PriorityQueue<uint, Point> openset;
        public BestFirstSearch(bool[,] unpassableNodes) : base(unpassableNodes) { }
        protected override void Initialize()
        {
            base.Initialize();
            openset = new PriorityQueue<uint, Point>();
        }
    }
}
