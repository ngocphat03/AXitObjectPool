namespace AXitUnityTemplate.ObjectPool.Runtime.Utilities
{
    using System.Linq;
    using UnityEngine;
    using System.Collections.Generic;

    public class Pool : MonoBehaviour
    {
        public           GameObject       prefabKey;
        private readonly List<GameObject> poolObjects    = new();
        private readonly List<GameObject> spawnedObjects = new();

        public void CreatePool(int size)
        {
            for (var i = 0; i < size; i++)
            {
                var obj = Object.Instantiate(this.prefabKey, this.transform);
                obj.SetActive(false);
                this.poolObjects.Add(obj);
            }
        }

        public GameObject GetAndSetObject(Transform parent = default, Vector3 position = default, Quaternion rotation = default)
        {
            var pool = this.poolObjects.FirstOrDefault();

            if (pool != null)
            {
                // Setup pool
                pool.transform.SetParent(parent);
                pool.transform.position = position;
                pool.transform.rotation = rotation;
                pool.SetActive(true);

                this.poolObjects.Remove(pool);
                this.spawnedObjects.Add(pool);

                return pool;
            }

            var newObj = Object.Instantiate(this.prefabKey, position, rotation, parent);
            this.spawnedObjects.Add(newObj);

            return newObj;
        }

        public void Recycle(GameObject obj)
        {
            obj.SetActive(false);
            this.poolObjects.Add(obj);
            this.spawnedObjects.Remove(obj);
        }

        public void ClearPool()
        {
            foreach (var obj in this.poolObjects.Concat(this.spawnedObjects).ToList())
            {
                Object.Destroy(obj);
            }

            this.poolObjects.Clear();
            this.spawnedObjects.Clear();
        }

        public int GetPoolSize() => this.poolObjects.Count;

        public int GetActiveCount() => this.spawnedObjects.Count;
    }
}