namespace AXitUnityTemplate.ObjectPool.Runtime.Manager
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using AXitUnityTemplate.ObjectPool.Runtime.Utilities;
    using AXitUnityTemplate.AssetLoader.Runtime.Interface;

    public class ObjectPoolManager
    {
        private readonly IAssetLoader assetLoader;
        private readonly PoolRoot     poolRoot;
        private readonly List<Pool>   pools = new();

        public ObjectPoolManager(IAssetLoader assetLoader,
                                 PoolRoot     poolRoot)
        {
            this.assetLoader = assetLoader;
            this.poolRoot    = poolRoot;
        }

        public async void CreatePool(string key, int size = 10, Action onComplete = null)
        {
            try
            {
#if UNITASK
                var prefab = await this.assetLoader.LoadAssetAsync<GameObject>(key).ToUniTask();
#else // Use coroutine
#endif
                this.CreatePool(prefab, size, onComplete);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to create pool: " + e.Message);
            }
        }

        public Pool CreatePool(GameObject prefab, int size = 10, Action onComplete = null)
        {
            var newPool = new GameObject(name: nameof(Pool), components: typeof(Pool)).GetComponent<Pool>();
            newPool.transform.SetParent(this.poolRoot.TransformPoolParent);
            newPool.transform.localPosition = Vector3.zero;
            newPool.prefabKey               = prefab;
            newPool.CreatePool(size);
            this.pools.Add(newPool);
            onComplete?.Invoke();

            return newPool;
        }

        public GameObject Spawn(GameObject prefab, Transform parent = default, Vector3 position = default, Quaternion rotation = default)
        {
            // Find Prefab
            var findPrefab = this.pools.Find(pool => pool.prefabKey == prefab);

            findPrefab ??= this.CreatePool(prefab);

            return findPrefab.GetAndSetObject(parent, position, rotation);
        }

        public void Recycle(GameObject prefab)
        {
            var findPrefab = this.pools.Find(pool => pool.prefabKey == prefab);
            if (findPrefab == null)
            {
                Debug.LogError("Failed to find pool: " + prefab);

                return;
            }

            findPrefab.Recycle(prefab);
        }
    }
}