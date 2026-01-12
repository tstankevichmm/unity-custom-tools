using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace CustomTools.Pooling
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField]
        private bool _addToDontDestroyOnLoad = false;

        private GameObject _holder;

        private static GameObject _particleSystemHolder;
        private static GameObject _gameObjectHolder;
        private static GameObject _soundFxHolder;

        private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
        private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

        public enum PoolType
        {
            GameObjects,
            ParticleSystem,
            SoundFX,
        }

        private void Awake()
        {
            _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
            _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

            SetupHolders();
        }

        private void SetupHolders()
        {
            _holder = new GameObject("Object Pools");
            CreateGameObject(out _gameObjectHolder, "Game Objects");
            CreateGameObject(out _particleSystemHolder, "Particle Effects");
            CreateGameObject(out _soundFxHolder, "Sound FX");
            
            if(_addToDontDestroyOnLoad)
                DontDestroyOnLoad(_holder.transform);
        }

        private void CreateGameObject(out GameObject go, string goName)
        {
            go = new GameObject(goName);
            go.transform.SetParent(_holder.transform);
        }

        private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => CreateObject(prefab, pos, rot, poolType),
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject
            );
            
            _objectPools.Add(prefab, pool);
        }

        private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
        {
            prefab.SetActive(false);
            
            GameObject obj = Instantiate(prefab, pos, rot);
            
            prefab.SetActive(true);
            
            GameObject parentObj = GetParentObject(poolType);
            obj.transform.SetParent(parentObj.transform);
            
            return obj;
        }

        private static void OnGetObject(GameObject go) { }  // No Op

        private static void OnReleaseObject(GameObject go) => go.SetActive(false);

        private static void OnDestroyObject(GameObject go)
        {
            if (_cloneToPrefabMap.ContainsKey(go))
            {
                _cloneToPrefabMap.Remove(go);
            }
        }
        
        private static T SpawnObject<T>(GameObject spawnObj, Vector3 pos, Quaternion rot, Transform parent, PoolType poolType = PoolType.GameObjects) where T : Object
        {
            GameObject obj = SpawnObject<GameObject>(spawnObj, pos, rot, poolType);
            obj.transform.SetParent(parent);
            return DetermineFinal<T>(obj);
        }

        private static T SpawnObject<T>(GameObject spawnObj, Transform parent, PoolType poolType = PoolType.GameObjects) where T : Object
        {
            GameObject obj = SpawnObject<GameObject>(spawnObj, parent.position, parent.rotation, poolType);
            obj.transform.SetParent(parent);
            return DetermineFinal<T>(obj);
        }

        private static T SpawnObject<T>(GameObject spawnObj, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) where T : Object
        {
            if (!_objectPools.ContainsKey(spawnObj))
            {
                CreatePool(spawnObj, pos, rot, poolType);
            }

            GameObject poolObj = _objectPools[spawnObj].Get();

            if (poolObj == null)
            {
                Debug.LogWarning($"Object {spawnObj.name} not found in pool.");
                return null;
            }

            if (!_cloneToPrefabMap.ContainsKey(poolObj))
            {
                Debug.Log($"Adding to pool, {poolObj.name} | {poolObj.GetHashCode()}");
                _cloneToPrefabMap.Add(poolObj, spawnObj);
            }

            poolObj.transform.position = pos;
            poolObj.transform.rotation = rot;
            poolObj.SetActive(true);

            return DetermineFinal<T>(poolObj);
        }

        public static T DetermineFinal<T>(GameObject go) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
                return go as T;

            T component = go.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"Object {go.name} ({go.GetHashCode()}) doesn't have a component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        public static T SpawnObject<T>(T prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) where T : Component => SpawnObject<T>(prefab.gameObject, pos, rot, poolType);

        public static GameObject SpawnObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) => SpawnObject<GameObject>(prefab, pos, rot, poolType);

        public static bool ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
        {
            if (!_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
            {
                Debug.LogWarning($"Trying to return an object that is not pooled: {obj.name} ({obj.GetHashCode()})");
                return false;
            }

            GameObject parentObject = GetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }

            return true;
        }
        
        private static GameObject GetParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.GameObjects:
                    return _gameObjectHolder;
                case PoolType.ParticleSystem:
                    return _particleSystemHolder;
                case PoolType.SoundFX:
                    return _soundFxHolder;
                default:
                    throw new ArgumentOutOfRangeException(nameof(poolType), poolType, null);
            }
        }
    }
}