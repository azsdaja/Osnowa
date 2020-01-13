namespace GameLogic.GameCore
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Core.ECS;
	using Osnowa.Osnowa.Core.ECS.Input;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;
	using Zenject;

	public class DirectionInputReader : MonoBehaviour
	{
		private GameContext _context;
		private IUnityGridInfoProvider _unityGridInfoProvider;
		private IUiFacade _uiFacade;

		[SerializeField] private DecisionInputReader _decisionInputReader;
		[SerializeField] private GameObject _aim;

		[Inject]
		public void Init(GameContext context, IUiFacade uiFacade, IUnityGridInfoProvider unityGridInfoProvider)
		{
			_context = context;
			_uiFacade = uiFacade;
			_unityGridInfoProvider = unityGridInfoProvider;
		}

		public void Update()
		{
			if (_aim.activeInHierarchy == false)
			{
				Position playerPosition = _context.GetPlayerEntity().position.Position;
				Vector3 playerWorldPosition = _unityGridInfoProvider.GetCellCenterWorld(playerPosition);
				_aim.transform.position = playerWorldPosition;
			}

			_aim.SetActive(true);

			bool noKeyIsPressed = !Input.anyKeyDown;
			if (noKeyIsPressed)
			{
				return;
			}


			_uiFacade.UiElementSelector.Deselect();
			Position direction = ReadDirection();

			if (direction == Position.Zero)
			{
				SwitchBackToDecisionMaking();
				return;
			}

			PlayerDecisionComponent decision = _context.playerDecision;
			_context.ReplacePlayerDecision(decision.Decision, direction, decision.Position);

			SwitchBackToDecisionMaking();
		}

		private void SwitchBackToDecisionMaking()
		{
			_aim.SetActive(false);
			_decisionInputReader.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}

		private Position ReadDirection()
		{
			if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.M))
			{
				return new Position(-1, -1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				return new Position(0, -1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Period))
			{
				return new Position(1, -1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				return new Position(-1, 0);
			}
			if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				return new Position(1, 0);
			}
			if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.U))
			{
				return new Position(-1, 1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				return new Position(0, 1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.O))
			{
				return new Position(1, 1);
			}

			return Position.Zero;
		}
	}
}