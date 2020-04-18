namespace GameLogic.Animation
{
	using Osnowa.Osnowa.Core;
	using Osnowa.Osnowa.Unity;
	using UnityEngine;
	using Zenject;

	/// <summary>
	/// Performs animations related to operations on the transform, e.g. moving, bumping.
	/// </summary>
	// todo: reorganize animations a bit. Use ECS? Separate single-shot animations (moving) from state-related animations (breathing, shaking)
	public class ActorAnimator : MonoBehaviour, IEntityAnimator
	{
		private IUnityGridInfoProvider _unityGridInfoProvider;
		private EntityAnimationState _animationState;
		private Animator _animator;

		public bool IsAnimating => _animationState != EntityAnimationState.Inactive;

	    public Transform Visuals;
		public Transform MyVisuals => Visuals;
		public Animator BodyAnimator;

		public float Progress;
		public Vector3 InitialPosition;
		public Vector3 AffectedPosition;

		private bool _startExecuted = false;

		void Awake()
		{
			_animator = GetComponent<Animator>();
			_startExecuted = true;
		}

		[Inject]
		public void Init(IUnityGridInfoProvider unityGridInfoProvider)
		{
			_unityGridInfoProvider = unityGridInfoProvider;
		}

		public void FinishedAnimating()
		{
			Progress = 0f;
			_animationState = EntityAnimationState.Inactive;
			InitialPosition = transform.position;
		}

		Animator IEntityAnimator.BodyAnimator => BodyAnimator;

		public void MoveTo(Position sourceLogicalPosition, Position targetLogicalPosition)
		{
			InitialPosition = _unityGridInfoProvider.GetCellCenterWorld(sourceLogicalPosition);
			AffectedPosition = _unityGridInfoProvider.GetCellCenterWorld(targetLogicalPosition);
			if (_animator == null)
			{
				Debug.LogWarning("missing animator; start executed = " + _startExecuted);
			}
			else
			{
				_animator.Play("GenericMove", 0, 0f);	
			}

			_animationState = EntityAnimationState.Moving;

		}

		public void Bump(Position sourceLogicalPosition, Position affectedPosition)
		{
			InitialPosition = _unityGridInfoProvider.GetCellCenterWorld(sourceLogicalPosition);
			AffectedPosition = _unityGridInfoProvider.GetCellCenterWorld(affectedPosition);
			_animator.Play("GenericBump", 0, 0f);

			_animationState = EntityAnimationState.Bumping;
		}

		public void KnockOut()
		{
			BodyAnimator.Play("DropUpsideDown");

			_animationState = EntityAnimationState.BeingKnockedOut;
		}

		public void StandUp()
		{
			_animationState = EntityAnimationState.StandingUp;
		}

		/// <remarks>
		/// For explanation why LateUpdate is used, see 
		/// https://forum.unity.com/threads/animator-locking-animated-value-even-when-current-state-has-no-curves-keys-for-that-value.440363/
		/// </remarks>
		void LateUpdate()
		{
			if (_animationState == EntityAnimationState.Inactive)
			{
				return;
			}
			if (_animationState == EntityAnimationState.Moving)
			{
				Visuals.position = Vector3.Lerp(InitialPosition, AffectedPosition, Progress);
			}
			if (_animationState == EntityAnimationState.Bumping)
			{
				Visuals.position = Vector3.Lerp(InitialPosition, AffectedPosition, Progress * 0.5f);
			}
		}
	}
}