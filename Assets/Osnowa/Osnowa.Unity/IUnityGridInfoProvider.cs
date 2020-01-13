using UnityEngine;

namespace Osnowa.Osnowa.Unity
{
	using Core;

	public interface IUnityGridInfoProvider
	{
		float CellSize { get; set; }
		Vector3Int LocalToCell(Vector3 localPosition);
		Vector3Int WorldToCell(Vector3 worldPosition);
		Vector3 GetCellCenterWorld(Position cellPosition);
	}
}