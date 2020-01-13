namespace UnityUtilities
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class PoolingManager : MonoBehaviour
	{
		[SerializeField] private EntityBehaviourPool _entityBehaviourPool;
		[SerializeField] private AbilityViewPool _abilityViewPool;

		private static readonly Dictionary<GameObject, Pool> Pools = new Dictionary<GameObject, Pool>();
		private static readonly IDictionary<string, GameObject> PooledTypesToPrefabs = new Dictionary<string, GameObject>();
		private static Dictionary<GameObject, Type> _prefabsToCustomPoolTypes;

		public static string EntityView = "EntityView";
		public static string StatusImage = "StatusImage";
		public static string PositionEffect = "PositionEffect";
		public static string EffectTilemap = "EffectTilemap";
		public static string AbilityView = "AbilityView";
		public static string Text = "Text";

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			_prefabsToCustomPoolTypes = new Dictionary<GameObject, Type>
			{
				{GetPrefab(EntityView), typeof(EntityBehaviourPool)},
				{GetPrefab(AbilityView), typeof(AbilityViewPool)},
			};

			// Zenject isn't able to initialize instances that are created during runtime, so we have to have them ready beforehand.
			Pools[GetPrefab(EntityView)] = _entityBehaviourPool;
			Pools[GetPrefab(AbilityView)] = _abilityViewPool;

			Initialized = true;
		}

		private static Pool CreatePool(GameObject prefab)
		{
			bool customPoolExists = _prefabsToCustomPoolTypes.ContainsKey(prefab);
			Type poolType = customPoolExists ? _prefabsToCustomPoolTypes[prefab] : typeof(Pool);
			var pool = new GameObject("Pool-" + prefab.name).AddComponent(poolType).GetComponent<Pool>();
			pool.Prefab = prefab;
			Pools.Add(prefab, pool);
			return pool;
		}

		private static Pool GetOrCreatePoolAndGrow(GameObject prefab)
		{
			if (Pools.ContainsKey(prefab))
				return Pools[prefab];

			Pool pool = CreatePool(prefab);

			pool.GrowPool();
			return pool;
		}

		public static TComponent Fetch<TComponent>(string key, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return Fetch(key, position, rotation, parent).GetComponent<TComponent>();
		}

		public static GameObject Fetch(string key, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			GameObject prefab = GetPrefab(key);
			Pool pool = GetOrCreatePoolAndGrow(prefab);
			GameObject gameObject = Fetch(key);

			if (parent != null)
				gameObject.transform.SetParent(parent);
			else
				gameObject.transform.SetParent(pool.transform);

			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			return gameObject;
		}

		public static TComponent Fetch<TComponent>(string key)
		{
			return Fetch(key).GetComponent<TComponent>();
		}

		public static GameObject Fetch(string key)
		{
			GameObject prefab = GetPrefab(key);
			Pool pool = GetOrCreatePoolAndGrow(prefab);
			GameObject gameObject = pool.Get();
			gameObject.SetActive(true);
			return gameObject;
		}


		public static void Free(string key, GameObject pooledObject)
		{
			GameObject prefab = GetPrefab(key);
			Pool pool = GetOrCreatePoolAndGrow(prefab);
			pooledObject.gameObject.SetActive(false);
			pool.DisabledObjects.Add(pooledObject);
			pool.AvailableObjects.Enqueue(pooledObject);
		}

		private static GameObject GetPrefab(string key)
		{
			GameObject prefab;
			bool prefabIsCached = PooledTypesToPrefabs.TryGetValue(key, out prefab);
			if (!prefabIsCached)
			{
				prefab = Resources.Load<GameObject>("Prefabs/Pooled/" + key);
			}
			if (prefab == null)
			{
				throw new InvalidOperationException($"Prefab with key {key} not found.");
			}
			PooledTypesToPrefabs[key] = prefab;
			return prefab;
		}
	}
}