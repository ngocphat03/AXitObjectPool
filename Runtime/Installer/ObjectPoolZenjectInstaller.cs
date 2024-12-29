#if ZENJECT
namespace AXitUnityTemplate.ObjectPool.Runtime.Installer
{
    using Zenject;
    using UnityEngine;
    using AXitUnityTemplate.ObjectPool.Runtime.Manager;
    using AXitUnityTemplate.ObjectPool.Runtime.Utilities;

    public class ObjectPoolZenjectInstaller : Installer<ObjectPoolZenjectInstaller>
    {
        private PoolRoot _poolRoot;
        
        public override void InstallBindings()
        {
            this.CreateRootPool();
            this.Container.BindInstance(this._poolRoot).AsSingle().NonLazy();
            this.Container.Bind<ObjectPoolManager>().AsSingle().NonLazy();
        }

        private void CreateRootPool()
        {
            this._poolRoot = new GameObject(name: nameof(PoolRoot), components: typeof(PoolRoot)).GetComponent<PoolRoot>();
            Object.DontDestroyOnLoad(this._poolRoot.gameObject);
            this._poolRoot.Init();
        }
    }
}
#endif