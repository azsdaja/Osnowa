namespace GameLogic.GameCore
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using GridRelated;
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Core.ECS.Input;
	using Osnowa.Osnowa.Fov;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using UnityUtilities;
	using Zenject;
	using Debug = UnityEngine.Debug;

	public class DirectionInputReader : MonoBehaviour
	{
		private GameContext _context;
		private IUnityGridInfoProvider _unityGridInfoProvider;
		private IUiFacade _uiFacade;
		private ITileMatrixUpdater _tileMatrixUpdater;
		private IRasterLineCreator _rasterLineCreator;
		private IGameConfig _gameConfig;

		[SerializeField] private DecisionInputReader _decisionInputReader;
		[SerializeField] private GameObject _aim;
		private IInputWithRepeating _inputWithRepeating;

		[Inject]
		public void Init(GameContext context, IUiFacade uiFacade, IUnityGridInfoProvider unityGridInfoProvider, ITileMatrixUpdater tileMatrixUpdater, 
			IRasterLineCreator rasterLineCreator, IGameConfig gameConfig, IInputWithRepeating inputWithRepeating)
		{
			_context = context;
			_uiFacade = uiFacade;
			_unityGridInfoProvider = unityGridInfoProvider;
			_tileMatrixUpdater = tileMatrixUpdater;
			_rasterLineCreator = rasterLineCreator;
			_gameConfig = gameConfig;
			_inputWithRepeating = inputWithRepeating;
		}

		private void OnEnable()
		{
			PlayerDecisionComponent decision = _context.playerDecision;
			Position playerPosition = _context.GetPlayerEntity().position.Position;
			_context.ReplacePlayerDecision(decision.Decision, Position.Zero, playerPosition);
		}

		public void Update()
		{
			Position playerPosition = _context.GetPlayerEntity().position.Position;
			if (_aim.activeInHierarchy == false)
			{
				Vector3 playerWorldPosition = _unityGridInfoProvider.GetCellCenterWorld(playerPosition);
				_aim.transform.position = playerWorldPosition;
			}

			_aim.SetActive(true);
			
			bool noKeyIsPressed = !Input.anyKey && !Input.anyKeyDown;//osnowatodo && _contextManager.Current.SimulatedKeyDown == KeyCode.None;
			if (noKeyIsPressed)
			{
				_inputWithRepeating.ResetTime();
				return;
			}

			_uiFacade.UiElementSelector.Deselect();
			Position direction = ReadDirection();
			if (direction == Position.MinValue) // escape
			{
				SwitchBackToDecisionMaking();
				return;
			}

			if (direction == Position.Zero)
			{
				// SwitchBackToDecisionMaking();
				return;
			}

			PlayerDecisionComponent decision = _context.playerDecision;
			Position newMarkerPosition = decision.Position + direction;
			_context.ReplacePlayerDecision(decision.Decision, direction, newMarkerPosition);
			PresentSelectionPath(playerPosition, newMarkerPosition);

			// SwitchBackToDecisionMaking();
		}

		private void PresentSelectionPath(Position playerPosition, Position newMarkerPosition)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			IList<Position> positions = _rasterLineCreator.GetRasterLine(playerPosition.x, playerPosition.y, newMarkerPosition.x, newMarkerPosition.y);
			
			// Debug.Log("raster: " + stopwatch.ElapsedMilliseconds);
			stopwatch.Restart();
			
			_tileMatrixUpdater.ClearForTile(_gameConfig.Tileset.SelectionPathMarker);
			// Debug.Log("clean: " + stopwatch.ElapsedMilliseconds);
			stopwatch.Restart();
			
			foreach (Position position in positions.Skip(1))
			{
				_tileMatrixUpdater.Set(position, _gameConfig.Tileset.SelectionPathMarker);
			}
			_tileMatrixUpdater.Set(positions.Last(), _gameConfig.Tileset.SelectionFinishMarker);

			// Debug.Log("set: " + stopwatch.ElapsedMilliseconds);
		}
		
		private void SwitchBackToDecisionMaking()
		{
			_tileMatrixUpdater.ClearForTile(_gameConfig.Tileset.SelectionPathMarker);
			_aim.SetActive(false);
			_decisionInputReader.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		private Position ReadDirection()
		{
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad1) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.M))
			{
				return new Position(-1, -1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad2) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.Comma) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.DownArrow))
			{
				return new Position(0, -1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad3) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.Period))
			{
				return new Position(1, -1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad4) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.J) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.LeftArrow))
			{
				return new Position(-1, 0);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad6) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.L) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.RightArrow))
			{
				return new Position(1, 0);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad7) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.U))
			{
				return new Position(-1, 1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad8) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.I) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.UpArrow))
			{
				return new Position(0, 1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Keypad9) || _inputWithRepeating.KeyDownOrRepeating(KeyCode.O))
			{
				return new Position(1, 1);
			}
			if (_inputWithRepeating.KeyDownOrRepeating(KeyCode.Escape))
			{
				return Position.MinValue;
			}
			return Position.Zero;
		}
	}
}