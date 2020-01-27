namespace Osnowa.Osnowa.Unity.Tiles.Scripts
{
	using UnityEngine;
	using UnityEngine.Tilemaps;

	public class OsnowaBaseTile : TileBase
	{
		public byte Id;

		/// <summary>
		/// At 0 — water layer.
		/// At 1 — dirt layer.
		/// At 2 — soil layer.
		/// At 3 — floor layer.
		/// At 4 — standing layer.
		/// At 5 — decoration layer.
		/// </summary>
		[Header(   "At 0 — water layer.\r\n" +
		            "At 1 — dirt layer.\r\n" +
		            "At 2 — soil layer.\r\n" +
		            "At 3 — floor layer.\r\n" +
		            "At 4 — standing layer.\r\n" +
		            "At 5 — decoration layer.")]
		public byte Layer;

		public OsnowaBaseTile ShorterVariant;

		public WalkabilityModifier Walkability = WalkabilityModifier.Indifferent;
		public PassingLightModifier IsPassingLight = PassingLightModifier.Indifferent;
	}

	public enum WalkabilityModifier
	{
		/// <summary>
		/// Doesn't change walkability of a position resolved on lower layers.
		/// </summary>
		Indifferent = 0,

		/// <summary>
		/// Forces the position to be walkable, no matter what was on lower layers. Still can be changed by walkability of upper layers.
		/// </summary>
		ForceWalkable = 1,

		/// <summary>
		/// Forces the position to be unwalkable, no matter what was on lower layers. Still can be changed by walkability of upper layers.
		/// </summary>
		ForceUnwalkable = 2
	}

	public enum PassingLightModifier
	{
		/// <summary>
		/// Doesn't change passing light of a position resolved on lower layers.
		/// </summary>
		Indifferent = 0,

		/// <summary>
		/// Forces the position to pass light, no matter what was on lower layers. Still can be changed by passing light of upper layers.
		/// </summary>
		ForcePassing = 1,

		/// <summary>
		/// Forces the position to pass light “poorly”, no matter what was on lower layers. Still can be changed by passing light of upper layers.
		/// </summary>
		ForcePoorPassing = 2,

		/// <summary>
		/// Forces the position to block light, no matter what was on lower layers. Still can be changed by passing light of upper layers 
		/// (e.g. by a window in a wall).
		/// </summary>
		ForceBlocking = 3
	}
}