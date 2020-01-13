using System.Collections.Generic;
using System.Linq;
using Osnowa;
using Osnowa.Osnowa.Core;
using Osnowa.Osnowa.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityUtilities;
using Zenject;

public class NumbersAreaPresenter : MonoBehaviour
{
	public Text Pattern;

	private IUnityGridInfoProvider _unityGridInfoProvider;
	private List<Text> _numbersInPool;

	[Inject]
	public void Init(IUnityGridInfoProvider unityGridInfoProvider)
	{
		_unityGridInfoProvider = unityGridInfoProvider;
		int numbersInitialCapacity = 100;
		_numbersInPool = new List<Text>(numbersInitialCapacity);
	}

	public void Show(IFloodArea floodArea)
	{
		if (!gameObject.activeInHierarchy)
			return;
		List<Text> numbersInPoolBeforeStart = _numbersInPool.ToList();
		foreach (Position position in floodArea.Bounds.AllPositions())
		{
			Position numberPosition = position;
			Vector3 worldPositionOfNewNumber = _unityGridInfoProvider.GetCellCenterWorld(numberPosition);
			Text textChild = PlaceChild(numbersInPoolBeforeStart, worldPositionOfNewNumber);
			int value = floodArea.GetValueAtPosition(position);
			textChild.text = value == int.MaxValue ? "-" : value.ToString();
		}
	}

	private Text PlaceChild(List<Text> numbersAvailableToPick, Vector3 position)
	{
		Text availableNumber = numbersAvailableToPick.FirstOrDefault();
		if (availableNumber == null)
		{
			availableNumber = GameObject.Instantiate(Pattern, position, Quaternion.identity, transform);
			_numbersInPool.Add(availableNumber);
		}
		else
		{
			numbersAvailableToPick.RemoveAt(0);
		}

		availableNumber.transform.position = position;
		availableNumber.color = Color.white;
		return availableNumber;
	}

	public void MarkWithColor(Dictionary<Position, int> positionsToDelays)
	{
		foreach (Text text in GetComponentsInChildren<Text>())
		{
			Position positionInGrid = _unityGridInfoProvider.WorldToCell(text.transform.position).ToPosition();
			if (positionsToDelays.ContainsKey(positionInGrid))
			{
				int delay = positionsToDelays[positionInGrid];
				Color color = delay == 0 ? Color.green : delay == 1 ? Color.yellow : delay == 2 ? Color.red : delay == 3 ? Color.gray : Color.black;
				text.color = color;
			}
		}
	}
}