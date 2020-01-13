using Libraries.Cinemachine.Base.Runtime.Behaviours;
using Osnowa.Osnowa.Unity;
using UnityEngine;

public class FollowedActorUpdater : MonoBehaviour
{
	private CinemachineVirtualCamera _followPlayerCamera;
		
	void Start()
	{
		_followPlayerCamera = GetComponent<CinemachineVirtualCamera>();
	}

	public void UpdateControlledActor(GameObject playerGameObject, GameEntity playerEntity)
	{
		_followPlayerCamera.Follow = playerGameObject.transform;
		EntityViewBehaviour playerEntityViewBehaviour = playerGameObject.GetComponent<EntityViewBehaviour>();
		
		// toecs tu bylo cos takiego:
		/*playerActor.GetComponent<EntityViewBehaviour>().ActorData.StealthScore.ValueChanged +=
				(float stealth) =>
				{
					float visibility = 1 - stealth;
					playerActor.GetComponent<ActorAnimator>().BodyAnimator.GetComponent<SpriteRenderer>().color = (Color.white * (visibility+0.01f));
				};*/
		// todo a ma byc cos w rodzaju zwiazania obecnej encji z miedzymordziem uzytkownika
		return;
	}
}