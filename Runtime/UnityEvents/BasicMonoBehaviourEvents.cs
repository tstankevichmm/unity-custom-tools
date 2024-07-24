using UnityEngine;
using UnityEngine.Events;

namespace CustomTools.UnityEvents
{
    public class BasicMonoBehaviourEvents : MonoBehaviour
    {
        public UnityEvent onStart;
        public UnityEvent onAwake;
        public UnityEvent onEnable;
        public UnityEvent onDisable;
        public UnityEvent onDestroy;

        private void Awake()
        {
            onAwake?.Invoke();
        }

        private void Start()
        {
            onStart?.Invoke();
        }

        private void OnEnable()
        {
            onEnable?.Invoke();
        }

        private void OnDisable()
        {
            onDisable?.Invoke();
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}