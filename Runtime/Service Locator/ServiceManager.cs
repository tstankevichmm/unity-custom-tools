using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.ServiceLocator
{
    public class ServiceManager
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public IEnumerable<object> RegisteredServices => _services.Values;

        public bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);

            if (_services.TryGetValue(type, out object obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        public T Get<T>() where T : class
        {
            if (TryGet(out T service)) 
                return service;
            
            Type type = typeof(T);
            throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} is not registered");
        }

        public ServiceManager Register<T>(T service, bool overrideCurrent)
        {
            Type type = typeof(T);
            return Register(type, service, overrideCurrent);
        }

        public ServiceManager Register(Type type, object service, bool overrideCurrent)
        {
            if (!type.IsInstanceOfType(service))
            {
                throw new ArgumentException("Type of service does not match type of service interface", nameof(service));
            }

            if (_services.ContainsKey(type))
            {
                if(overrideCurrent)
                {
                    DeRegisterCurrent(type);
                }
            }
            
            if (!_services.TryAdd(type, service))
            {
                Debug.LogError($"ServiceManager.Register: Error when trying to add type {nameof(type)}. Service of type {type.FullName} already registered");
            }

            return this;
        }

        public ServiceManager DeRegister(Type type, object service)
        {
            if (!_services.TryGetValue(type, out object obj))
            {
                Debug.LogError($"ServiceManager.DeRegister: Tried to DeRegister type {type.FullName}, but no object was registered with that type");
                return this;
            }

            if (service != obj)
            {
                Debug.LogError($"ServiceManager.DeRegister: Object passed in does not match the object that was registered");
            }
            
            _services.Remove(type);
            return this;
        }

        public ServiceManager DeRegisterCurrent(Type type)
        {
            _services.Remove(type);
            return this;
        }
    }
}
