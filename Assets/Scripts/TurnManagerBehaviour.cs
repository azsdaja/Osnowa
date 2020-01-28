using Osnowa.Osnowa.Core;
using UnityEngine;
using Zenject;

/// <summary>
/// Manages the game loop — gives control to specific actors when their turn comes.
/// </summary>
public class TurnManagerBehaviour : MonoBehaviour
{
	private ITurnManager _turnManager;

	[Inject]
	public void Init(ITurnManager turnManager)
	{
		_turnManager = turnManager;
	}

	void Start()
	{
		_turnManager.OnGameStart();
	}

	void LateUpdate()
	{
		// todo _contextManager.Current.SimulatedKeyDown = KeyCode.None;
	}

	void Update()
	{
		_turnManager.Update();
	}
}