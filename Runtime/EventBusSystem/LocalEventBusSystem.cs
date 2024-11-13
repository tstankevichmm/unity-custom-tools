using UnityEngine;

namespace CustomTools.EventBusSystem
{
    public class LocalEventBusSystem : MonoBehaviour, ILocalEventBusSystem
    {
        public LocalEventBus EventBus { get; private set; }

        private void Awake()
        {
            EventBus = new LocalEventBus();
        }
    }
}