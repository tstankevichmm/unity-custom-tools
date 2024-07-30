using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.ServiceLocator
{
    public class ServiceLocatorHelperGlobal : MonoBehaviour
    {
        [SerializeField] private List<Object> _services;

        private void Awake()
        {
            foreach (Object service in _services)
            {
                ServiceLocator.Global.Register(service.GetType(), service, false);
            }
        }
    }
}