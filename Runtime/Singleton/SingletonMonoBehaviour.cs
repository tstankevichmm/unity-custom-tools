using UnityEngine;

namespace CustomTools.Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool Exists => _instance != null;
        private static T _instance;
        private static bool _isQuitting;
        
        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<T>();

                if (_instance != null)
                    return _instance;

                if (_isQuitting)
                    return null;
                
                GameObject singleton = new GameObject(typeof(T).ToString());
                _instance = singleton.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        protected virtual void OnDestroy()
        {
            // During scene unload or quit, Unity destroys objects; treat that as quitting-safe.
            if (_instance == this)
                _instance = null;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            // Important for Domain Reload disabled / Enter Play Mode Options
            _instance = null;
            _isQuitting = false;

            Application.quitting -= OnAppQuitting;
            Application.quitting += OnAppQuitting;
        }
        
        private static void OnAppQuitting()
        {
            _isQuitting = true;
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}