namespace UnityUtilities
{
	using System.Collections.Generic;
	using UnityEngine;

	public class Pool : MonoBehaviour
	{
		private int _poolSize;
		private const int QueueCapacityIncrease = 10;

		public Queue<GameObject> AvailableObjects { get; } = new Queue<GameObject>(QueueCapacityIncrease);
		public List<GameObject> DisabledObjects { get; } = new List<GameObject>();
		public GameObject Prefab { get; set; }

		void Update()
		{
			MakeDisabledObjectsChildren();
		}

		public GameObject Get()
		{
			if (AvailableObjects.Count == 0)
			{
				GrowPool();
			}

			GameObject pooledObject = AvailableObjects.Dequeue();
			return pooledObject;
		}

		public void GrowPool()
		{
			for (int i = _poolSize; i < _poolSize + QueueCapacityIncrease; i++)
			{
				GameObject pooledObject = InstantiateGameObject();
				pooledObject.name += " " + i;
				pooledObject.SetActive(false);
				pooledObject.transform.SetParent(transform);
				AvailableObjects.Enqueue(pooledObject);
			}
			_poolSize += QueueCapacityIncrease;
		}

		protected virtual GameObject InstantiateGameObject()
		{
			return Instantiate(Prefab);
		}

		private void MakeDisabledObjectsChildren()
		{
			if (DisabledObjects.Count > 0)
			{
				foreach (GameObject pooledObject in DisabledObjects)
				{
					if (pooledObject.gameObject.activeInHierarchy == false)
					{
						pooledObject.transform.SetParent(transform);
					}
				}

				DisabledObjects.Clear();
			}
		}
	}
}