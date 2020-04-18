namespace Osnowa.Osnowa.Tiles
{
	using Unity.Tiles.Scripts;
	using UnityEngine;

	[CreateAssetMenu(fileName = nameof(Tileset), menuName = "Osnowa/Configuration/Tileset", order = 0)]
	public class Tileset : ScriptableObject
	{
		public OsnowaBaseTile FogOfWar;
		public OsnowaBaseTile UnseenMask;
        public OsnowaBaseTile DebugWhite;
        public OsnowaBaseTile SelectionPathMarker;
        public OsnowaBaseTile SelectionFinishMarker;
        public OsnowaBaseTile VisibleByOthers;

		public OsnowaBaseTile Sand;
		public OsnowaBaseTile DryDirt;
		public OsnowaBaseTile Soil;
		public OsnowaBaseTile SaltyWater;
	
		public OsnowaBaseTile Roof;

        public OsnowaBaseTile Wall;

        public OsnowaBaseTile[] OtherTiles;
	}
}