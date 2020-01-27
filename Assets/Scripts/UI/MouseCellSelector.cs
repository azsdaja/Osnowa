namespace UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using GameLogic;
	using Osnowa.Osnowa.Context;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Grid;
	using Osnowa.Osnowa.Unity;
	using Osnowa.Osnowa.Unity.Tiles;
	using Osnowa.Osnowa.Unity.Tiles.Scripts;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityUtilities;
	using Zenject;

	public class MouseCellSelector : MonoBehaviour
	{
		[SerializeField] private Image _selectionImage;
		private Vector3 _lastMousePositionOnScreen;

		private IUnityGridInfoProvider _unityGridInfoProvider;
		private IUiFacade _uiFacade;
		private IOsnowaContextManager _contextManager;
		private ITileByIdProvider _tileByIdProvider;
		private IGameConfig _gameConfig;
		private IEntityDetector _entityDetector;
		private GameContext _context;

		[Inject]
		public void Init(IUnityGridInfoProvider unityGridInfoProvider, IUiFacade uiFacade, IOsnowaContextManager contextManager, 
			ITileByIdProvider tileByIdProvider, IGameConfig gameConfig, IEntityDetector entityDetector, GameContext context)
		{
			_tileByIdProvider = tileByIdProvider;
			_contextManager = contextManager;
			_uiFacade = uiFacade;
			_unityGridInfoProvider = unityGridInfoProvider;
			_gameConfig = gameConfig;
			_entityDetector = entityDetector;
			_context = context;
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 mousePositionOnScreen = Input.mousePosition;
			Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
			Position mousePosition = _unityGridInfoProvider.WorldToCell(worldMousePosition).ToPosition();
			Vector3 selectorWorldPosition = _unityGridInfoProvider.GetCellCenterWorld(mousePosition);
			if (_lastMousePositionOnScreen != mousePositionOnScreen)
			{
				_lastMousePositionOnScreen = mousePositionOnScreen;
				_selectionImage.transform.position = selectorWorldPosition;

				if (!_contextManager.Current.PositionFlags.IsWithinBounds(mousePosition))
				{
					return;
				}
				var positionText = new StringBuilder();
				GameEntity entityAtCursor = GetEntityAtCursor(mousePosition);
				if (entityAtCursor != null)
				{
					positionText.Append(entityAtCursor.recipee.RecipeeName + Environment.NewLine);
				}

				_uiFacade.ShowEntityDetails(entityAtCursor);
				AddTerrainText(mousePosition, positionText);
				_uiFacade.SetHoveredPositionText(positionText.ToString());
			}
		}

		private GameEntity GetEntityAtCursor(Position mousePosition)
		{
			List<GameEntity> eligibleEntities = _entityDetector.DetectEntities(mousePosition)
                .Where(e => _contextManager.Current.VisibleEntities.Contains(e.view.Controller)).ToList();
			return eligibleEntities.FirstOrDefault();
		}

		private void AddTerrainText(Position mousePosition, StringBuilder positionText)
		{
			IOsnowaContext context = _contextManager.Current;
			byte standingIdAtPosition = context.TileMatricesByLayer[TilemapLayers.Standing].Get(mousePosition);
			byte floorIdAtPosition = context.TileMatricesByLayer[TilemapLayers.Floor].Get(mousePosition);
			byte soilIdAtPosition = context.TileMatricesByLayer[TilemapLayers.Soil].Get(mousePosition);

			OsnowaBaseTile[] tilesByIds = _tileByIdProvider.GetTilesByIds(_gameConfig.Tileset);

			OsnowaBaseTile standingBaseTile = tilesByIds[standingIdAtPosition];
			OsnowaBaseTile floorBaseTile = tilesByIds[floorIdAtPosition];
			OsnowaBaseTile soilBaseTile = tilesByIds[soilIdAtPosition];

			if (standingBaseTile != null)
				positionText.Append(standingBaseTile.name + ". " + Environment.NewLine);
			if (floorBaseTile != null)
				positionText.Append(floorBaseTile.name + ". " + Environment.NewLine);
			if (soilBaseTile != null)
				positionText.Append(soilBaseTile.name + ". " + Environment.NewLine);

			positionText.Append(mousePosition);
		}
	}
}