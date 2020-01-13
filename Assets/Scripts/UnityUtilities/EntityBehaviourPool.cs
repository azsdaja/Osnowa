namespace UnityUtilities
{
	using Osnowa.Osnowa.Unity;
	using UnityEngine;
	using Zenject;

	public class EntityBehaviourPool : Pool
	{
		private EntityViewBehaviour.Factory _entityBehaviourFactory;

		[Inject]
		public void Initialize(EntityViewBehaviour.Factory entityBehaviourFactory)
		{
			_entityBehaviourFactory = entityBehaviourFactory;
		}

		protected override GameObject InstantiateGameObject()
		{
			return _entityBehaviourFactory.Create().gameObject;
		}
	}
}