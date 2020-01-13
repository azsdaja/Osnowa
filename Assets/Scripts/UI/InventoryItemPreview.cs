namespace UI
{
	using GameLogic;
	using Osnowa.Osnowa.Context;
	using UnityEngine;
	using UnityEngine.UI;
	using Zenject;

	public class InventoryItemPreview : MonoBehaviour
	{
		public Image Image;
		public RectTransform Rect;
		public GameEntity ItemEntity { get; set; }
		public int Index;

		private IUiFacade _uiFacade;
		private IOsnowaContextManager _contextManager;

		[Inject]
		public void Init(IUiFacade uiFacade, IOsnowaContextManager contextManager)
		{
			_uiFacade = uiFacade;
			_contextManager = contextManager;
		}

		public void PointerEnter()
		{
			_uiFacade.ShowEntityDetails(ItemEntity);
		}

		public void PointerExit()
		{
			_uiFacade.ShowEntityDetails(null);
		}

		public void Click()
		{
			/*KeyCode itemChoiceKey;
			if(Enum.TryParse("Alpha" + (Index + 1), out itemChoiceKey))
			{
				_contextManager.Current.SimulatedKeyDown = itemChoiceKey;
			}*/
		}
	}
}