using Osnowa.Osnowa.Context;
using UnityEngine;
using UnityUtilities;

namespace Osnowa.Osnowa.Unity
{
	using Core;

	public class UnityGridInfoProvider : IUnityGridInfoProvider
	{
		private readonly ISceneContext _sceneContext;
		private PositionFlags _positionFlags;
		public int XSize => _positionFlags.XSize;
		public int YSize => _positionFlags.YSize;
		public Position MinPosition { get; }
		public float CellSize { get; set; }

		public UnityGridInfoProvider(ISceneContext sceneContext, IOsnowaContextManager contextManager)
		{
			_sceneContext = sceneContext;

			contextManager.ContextReplaced += newContext => _positionFlags = newContext.PositionFlags;

			_sceneContext.TilemapDefiningOuterBounds.CompressBounds();
			MinPosition = Position.Zero;
			CellSize = _sceneContext.TilemapDefiningOuterBounds.cellSize.x;
		}

		public Vector3Int LocalToCell(Vector3 localPosition)
		{
			return _sceneContext.GameGrid.LocalToCell(localPosition);
		}

		public Vector3Int WorldToCell(Vector3 worldPosition)
		{
			return _sceneContext.GameGrid.WorldToCell(worldPosition);
		}

		public Vector3 GetCellCenterWorld(Position cellPosition)
		{
			return _sceneContext.GameGrid.GetCellCenterWorld(cellPosition.ToVector3Int());
		}
	}
}