using System;
using UnityEngine;
using UnityEngine.Events;

namespace CustomTools.Observer
{
    [Serializable]
    public class ObserverValue<T>
    {
        public T Value
        {
            get => _value;
            set => Set(value);
        }
        
        [SerializeField] 
        private T _value;
        [NonSerialized]
        private UnityEvent<T> _onValueChanged;

        public ObserverValue(T value, UnityAction<T> callback = null)
        {
            _value = value;
            _onValueChanged = new UnityEvent<T>();
            
            if(callback != null)
                _onValueChanged.AddListener(callback);
        }
    
        public static implicit operator T(ObserverValue<T> observer) => observer.Value;

        private void Set(T value)
        {
            if (Equals(_value, value))
                return;
            
            _value = value;
            Invoke();
        }

        public void Invoke()
        {
            _onValueChanged.Invoke(_value);
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
            return _value.ToString();
        }
    }
}