using UnityEngine;

namespace CustomTools.ServiceLocator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class Bootstrapper : MonoBehaviour
    {
        internal ServiceLocator Container
        {
            get
            {
                if (_container == null) 
                    _container = GetComponent<ServiceLocator>();
                
                return _container;
            }
        }
        
        private ServiceLocator _container;
        private bool _hasBeenBootstrapped;

        private void Awake()
        {
            BootstrapOnDemand();
        }

        public void BootstrapOnDemand()
        {
            if (_hasBeenBootstrapped)
                return;

            _hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}