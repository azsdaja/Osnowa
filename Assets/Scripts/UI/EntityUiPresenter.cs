namespace UI
{
	using System.Collections.Generic;
	using System.Linq;
	using GameLogic.Entities;
	using Osnowa.NaPozniej;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityUtilities;
	using Zenject;

	public class EntityUiPresenter : MonoBehaviour, IEntityUiPresenter
	{
		private IGameConfig _gameConfig;	

		[SerializeField] private VerticalLayoutGroup _statusesGroup;

		[SerializeField] private GenericBar _integrityBar;
		private readonly Dictionary<ViewStatusClass, Image> _classesToStatusImages;

		public EntityUiPresenter()
		{
			_classesToStatusImages = new Dictionary<ViewStatusClass, Image>();
		}

		[Inject]
		public void Init(IGameConfig gameConfig)
		{
			_gameConfig = gameConfig;
		}

		public void SetStatus(ViewStatusClass viewStatusClass, ActorStatusDefinition statusDefinition)
		{
			Image statusImage;
			if (_classesToStatusImages.ContainsKey(viewStatusClass))
				statusImage = _classesToStatusImages[viewStatusClass];
			else
			{
				statusImage = PoolingManager.Fetch(PoolingManager.StatusImage, _statusesGroup.transform.position, 
								 Quaternion.identity, _statusesGroup.transform)
										.GetComponent<Image>();
				_classesToStatusImages[viewStatusClass] = statusImage;
			}
			statusImage.sprite = statusDefinition.Icon; 
			statusImage.transform.localPosition = Vector3.zero;
			statusImage.transform.localScale = new Vector3(1, 1, 1);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_statusesGroup.GetComponent<RectTransform>()); // prevents statuses from laying one on top of another
		}

		public void RemoveStatus(ViewStatusClass viewStatusClass)
		{
			Image imageToFree;
			if (_classesToStatusImages.TryGetValue(viewStatusClass, out imageToFree))
			{
				_classesToStatusImages.Remove(viewStatusClass);
				PoolingManager.Free(PoolingManager.StatusImage, imageToFree.gameObject);
			}
		}

		public void SetIntegrityRatio(float integrityRatio)
		{
			_integrityBar.gameObject.SetActive(integrityRatio < 1f);

			_integrityBar.OnChanged(integrityRatio);
		}

		public void SetProgressRatio(float progress)
		{
			_integrityBar.gameObject.SetActive(progress < 1f);

			_integrityBar.OnChanged(progress);
		}

		public void FreeTemporaryUiElements()
		{
			IEnumerable<GameObject> statusesToFree = _statusesGroup.GetComponentsInChildren<Image>().Select(i => i.gameObject);
			foreach (GameObject statusObject in statusesToFree)
			{
				PoolingManager.Free(PoolingManager.StatusImage, statusObject);
			}
		}
	}
}
