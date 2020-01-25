namespace Osnowa.Osnowa.Tiles
{
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using UnityEngine;

	[CreateAssetMenu(fileName = nameof(Tileset), menuName = "Osnowa/Configuration/Tileset", order = 0)]
	public class Tileset : ScriptableObject
	{
		public KafelkiTile FogOfWar;
		public KafelkiTile UnseenMask;
        public KafelkiTile DebugWhite;

		public KafelkiTile Sand;
		public KafelkiTile DryDirt;
		public KafelkiTile Soil;
		public KafelkiTile SaltyWater;
	
		public KafelkiTile Roof;

        public KafelkiTile Wall;

        public KafelkiTile[] OtherTiles;
	}
}