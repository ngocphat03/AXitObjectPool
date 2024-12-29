namespace AXitUnityTemplate.ObjectPool.Runtime.Utilities
{
    using UnityEngine;

    public class PoolRoot : MonoBehaviour
    {
        public Transform TransformPoolParent { get; private set; }

        public void Init()
        {
            // Create parent pool
            this.TransformPoolParent = new GameObject(name: nameof(this.TransformPoolParent)).transform;
            this.TransformPoolParent.SetParent(this.transform);
        }
    }
}