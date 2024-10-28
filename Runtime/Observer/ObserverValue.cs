using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CustomTools.Observer
{
    [Serializable]
    public class ObserverValue<T>
    {
        public virtual T Value
        {
            get => _baseValue;
            set => Set(value);
        }
        
        [FormerlySerializedAs("_value")] [SerializeField] 
        protected T _baseValue;
        [NonSerialized]
        protected UnityEvent<T> _onValueChanged;

        public ObserverValue(T baseValue, UnityAction<T> callback = null)
        {
            _baseValue = baseValue;
            _onValueChanged = new UnityEvent<T>();
            
            if(callback != null)
                _onValueChanged.AddListener(callback);
        }
    
        public static implicit operator T(ObserverValue<T> observer) => observer.Value;

        private void Set(T value)
        {
            if (Equals(_baseValue, value))
                return;
            
            _baseValue = value;
            Invoke();
        }

        public void Invoke()
        {
            _onValueChanged.Invoke(Value);
        }

        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null)
                return;
            
            _onValueChanged.AddListener(callback);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null)
                return;
            
            _onValueChanged.RemoveListener(callback);
        }
    
        public override string ToString()
        {
            return _baseValue.ToString();
        }
    }
}