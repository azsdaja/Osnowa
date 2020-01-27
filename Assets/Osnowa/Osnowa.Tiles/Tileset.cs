namespace Osnowa.Osnowa.Tiles
{
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using UnityEngine;

	[CreateAssetMenu(fileName = nameof(Tileset), menuName = "Osnowa/Configuration/Tileset", order = 0)]
	public class Tileset : ScriptableObject
	{
		public OsnowaBaseTile FogOfWar;
		public OsnowaBaseTile UnseenMask;
        public OsnowaBaseTile DebugWhite;

		public OsnowaBaseTile Sand;
		public OsnowaBaseTile DryDirt;
		public OsnowaBaseTile Soil;
		public OsnowaBaseTile SaltyWater;
	
		public OsnowaBaseTile Roof;

        public OsnowaBaseTile Wall;

        public OsnowaBaseTile[] OtherTiles;
	}
}