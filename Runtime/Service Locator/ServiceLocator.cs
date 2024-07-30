using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomTools.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global;
        private static Dictionary<Scene, ServiceLocator> _sceneContainers;

        private readonly ServiceManager _serviceManager = new ServiceManager();

        private const string GlobalServiceLocatorName = "ServiceLocator [Global]";
        private const string SceneServiceLocatorName = "ServiceLocator [Scene]";

        private static List<GameObject> _tempSceneGameObjects = new List<GameObject>();

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (_global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global", this);
            }
            else if (_global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
            }
            else
            {
                _global = this;
                if(dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (_sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene", this);
            }
            else
            {
                _sceneContainers.Add(scene, this);
            }
        }
        
        public static ServiceLocator Global
        {
            get
            {
                //Check if it is already defined
                if (_global != null)
                    return _global;
                
                //Look for a global bootstrapper and return that
                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return _global;
                }

                //Finally create a bootstrapper and run it if all else fails
                GameObject container = new GameObject(GlobalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
                return _global;
            }
        }

        public static ServiceLocator For(MonoBehaviour monoBehaviour)
        {
            ServiceLocator serviceLocator = monoBehaviour.GetComponentInParent<ServiceLocator>();

            if (serviceLocator != null)
                return serviceLocator;

            return ForSceneOf(monoBehaviour);
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour)
        {
            Scene scene = monoBehaviour.gameObject.scene;

            if (_sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != monoBehaviour)
            {
                return container;
            }
            
            _tempSceneGameObjects.Clear();
            
            if (!scene.IsValid())
                return Global;
            
            scene.GetRootGameObjects(_tempSceneGameObjects);

            foreach (GameObject sceneGameObject in _tempSceneGameObjects)
            {
                if(!sceneGameObject.TryGetComponent<ServiceLocatorSceneBootstrapper>(out ServiceLocatorSceneBootstrapper sceneBootstrapper) || sceneBootstrapper.Container == monoBehaviour)
                    continue;
                
                sceneBootstrapper.BootstrapOnDemand();
                return sceneBootstrapper.Container;
            }

            return Global;
        }

        public ServiceLocator Register(Type type, object service, bool overrideCurrent)
        {
            Debug.Log($"Registering type {type.FullName}");
            _serviceManager.Register(type, service, overrideCurrent);
            return this;
        }

        public ServiceLocator DeRegister(Type type, object service)
        {
            Debug.Log($"DeRegistering type {type.FullName}");
            _serviceManager.DeRegister(type, service);
            return this;
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service))
                return this;

            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get<T>(out service);
                return this;
            }

            Debug.LogError($"ServiceLocator.Get: Service of type {typeof(T).FullName} is not registered.");
            return this;
        }

        public bool TryGetService<T>(out T service) where T : class
        {
            return _serviceManager.TryGet(out service);
        }

        public bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == _global)
            {
                container = null;
                return false;
            }

            container = transform.parent == null ? ForSceneOf(this) : transform.parent.GetComponentInParent<ServiceLocator>();
            return container != null;
        }

        private void OnDestroy()
        {
            if (this == _global)
            {
                _global = null;
            }
            else if (_sceneContainers.ContainsValue(this))
            {
                _sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _global = null;
            _sceneContainers = new Dictionary<Scene, ServiceLocator>();
            _tempSceneGameObjects = new List<GameObject>();
        }

        #if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        private static void AddGlobal()
        {
            GameObject container = new GameObject(GlobalServiceLocatorName, typeof(ServiceLocator));
            container.AddComponent<ServiceLocatorGlobalBootstrapper>();
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        private static void AddScene()
        {
            GameObject container = new GameObject(SceneServiceLocatorName, typeof(ServiceLocator));
            container.AddComponent<ServiceLocatorSceneBootstrapper>();
        }
        #endif
    }
}