namespace Osnowa.Osnowa.Unity
{
	using System;
	using System.Linq;
	using Core;
	using Entitas.VisualDebugging.Unity;
	using GameLogic;
	using GameLogic.Animation;
	using GameLogic.Entities;
	using NaPozniej;
	using UI;
	using UnityEditor;
	using UnityEngine;
	using UnityUtilities;
	using Zenject;

	/// <summary>
	/// A basic being in the game, exposing some aspects of Unity's MonoBehaviour representing a single actor or item.
	/// </summary>
	public class EntityViewBehaviour : MonoBehaviour, IViewController
	{
		[SerializeField] private EntityUiPresenter _entityUiPresenter;
		[SerializeField] private SpriteRenderer _bodySpriteRenderer;
		[SerializeField] private ActorAnimator _entityAnimator;
		[SerializeField] private GameEntity _entity;
		[SerializeField] private PositionedEntity _positionedEntity;

		private IUiFacade _uiFacade;
		private GameContext _context;
		private ISceneContext _sceneContext;
		private IUnityGridInfoProvider _unityGridInfoProvider;
		private IGameConfig _gameConfig;

		public IEntityUiPresenter EntityUiPresenter => _entityUiPresenter;

		[Inject]
		public void Init(ISceneContext sceneContext, IUnityGridInfoProvider unityGridInfoProvider, IGameConfig gameConfig)
		{
			_sceneContext = sceneContext;
			_unityGridInfoProvider = unityGridInfoProvider;
			_gameConfig = gameConfig;
        }

		void Awake()
		{
			_bodySpriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
			_entityAnimator = GetComponent<ActorAnimator>();
			UiPresenter = GetComponent<EntityUiPresenter>();
		}

		void Start()
		{
			gameObject.SetActive(true);
		}

		public Transform Transform => transform;
		public IEntityAnimator Animator => _entityAnimator;

		public int SortingOrder
		{
			get { return _bodySpriteRenderer.sortingOrder; }
			set { _bodySpriteRenderer.sortingOrder = value; }
		}

		public IEntityUiPresenter UiPresenter { get; private set; }

		public string Name
		{
			get { return gameObject.name; }
			set { gameObject.name = value; }
		}

		public void SetAsActiveActor() 
		{
			FindObjectOfType<FollowedActorUpdater>().UpdateControlledActor(gameObject, Entity);
		}

		public void SetStatus(ViewStatusClass viewStatusClass, ActorStatusDefinition statusDefinition)
		{
			try
			{
				if (statusDefinition == null)
				{
					EntityUiPresenter.RemoveStatus(viewStatusClass);
				}
				else
				{
					EntityUiPresenter.SetStatus(viewStatusClass, statusDefinition);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message + ", stack trace: " + e.StackTrace);
			}
		}

		public void Free()
		{
			_entityUiPresenter.FreeTemporaryUiElements();
			PoolingManager.Free(PoolingManager.EntityView, gameObject);
		}

	    public GameEntity Entity
		{
			get { return _entity; }
	        set
	        {
	            _entity = value;
	            PositionedEntity = new PositionedEntity(value);
	        }
		}

	    public PositionedEntity PositionedEntity
		{
			get { return _positionedEntity; }
			set { _positionedEntity = value; }
		}

		public class Factory : PlaceholderFactory<EntityViewBehaviour>
		{
		}

	    public Position Position => _entity.position.Position;

	    public virtual void Show()
		{
			if (_bodySpriteRenderer == null)
			{
				Awake(); // Show() can be called before Start is.
			}

			//if (!Entity.isPlayerControlled)
			//	EntityUiPresenter.ShowDetails();
			_bodySpriteRenderer.enabled = true;
			_entityAnimator.MyVisuals.gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			_entityAnimator.FinishedAnimating();

			_bodySpriteRenderer.enabled = false;
			_entityAnimator.MyVisuals.gameObject.SetActive(false);
		}

		public void HoldOnFront(GameEntity entityToHold)
		{
			var entityView = (EntityViewBehaviour)entityToHold.view.Controller;
			entityView.Show();
			entityView.Transform.SetParent((_entityAnimator).BodyAnimator.transform);
			entityView.SortingOrder = SortingOrder + 1;
			entityView.Transform.localPosition = new Vector3(-0.08f, 0.05f, 0f);
		}

		public void HoldOnBack(GameEntity entityToHold)
		{
			var entityView = (EntityViewBehaviour)entityToHold.view.Controller;

			entityView.Transform.SetParent((_entityAnimator).BodyAnimator.transform);
			entityView.SortingOrder = SortingOrder - 1;
			entityView.Transform.localPosition = new Vector3(-0.05f, 0.04f, 0f);
			entityView.Transform.rotation = Quaternion.Euler(0, 0, 12);
		}

		public void DropHeldEntity(GameEntity entityToDrop)
		{
			var entityView = (EntityViewBehaviour)entityToDrop.view.Controller;

			entityView.Show();
			entityView.Transform.SetParent(_sceneContext.EntitiesParent);
			entityView.Transform.rotation = Quaternion.identity;
			entityView.SortingOrder = SortingOrder - 1;
		}

		public bool IsVisible => _bodySpriteRenderer.enabled;

		public void RefreshWorldPosition()
		{
			transform.position = _unityGridInfoProvider.GetCellCenterWorld(Entity.position.Position);
		}

		public Sprite GetSprite()
		{
			return _bodySpriteRenderer.sprite;
		}

		public void SetSprite(Sprite sprite, Color? color = null)
		{
			_bodySpriteRenderer.sprite = sprite;
			if (color.HasValue)
			{
				_bodySpriteRenderer.color = color.Value;
			}
		}

		public void OnMouseOver()
		{
			if (Input.GetMouseButtonDown(0))
			{
				EntityBehaviour matchingEntityBehaviour = FindObjectsOfType<EntityBehaviour>()
					.FirstOrDefault(b => b.entity == Entity);
				if (matchingEntityBehaviour != null)
				{
#if UNITY_EDITOR
					Selection.activeGameObject = matchingEntityBehaviour.gameObject;
#endif
				}
			}
		}
	}
}