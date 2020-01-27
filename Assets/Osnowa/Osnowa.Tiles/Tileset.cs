namespace Osnowa.Osnowa.Tiles
{
	using Assets.Plugins.TilemapEnhancements.Tiles.Rule_Tile.Scripts;
	using UnityEngine;

	[CreateAssetMenu(fileName = nameof(Tileset), menuName = "Osnowa/Configuration/Tileset", order = 0)]
	public class Tileset : ScriptableObject
	{
		public OsnowaTile FogOfWar;
		public OsnowaTile UnseenMask;
        public OsnowaTile DebugWhite;

		public OsnowaTile Sand;
		public OsnowaTile DryDirt;
		public OsnowaTile Soil;
		public OsnowaTile SaltyWater;
	
		public OsnowaTile Roof;

        public OsnowaTile Wall;

        public OsnowaTile[] OtherTiles;
	}
}