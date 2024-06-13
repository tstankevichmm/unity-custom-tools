using System;

namespace CustomTools.EventBusSystem
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        public int Priority { get; set; }
        public Action<T, Action> OnEventCallback { get; set; }
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }

        public EventBinding(Action<T, Action> onEventCallback, int priority = 0, bool autoRegisterGlobally = false)
        {
            OnEventCallback = onEventCallback;
            Priority = priority;
            
            if(autoRegisterGlobally)
                AutoRegisterGlobally();
        }

        public EventBinding(Action<T> onEvent, int priority = 0, bool autoRegisterGlobally = false)
        {
            OnEvent = onEvent;
            Priority = priority;
            
            if(autoRegisterGlobally)
                AutoRegisterGlobally();
        }

        public EventBinding(Action onEventNoArgs, int priority = 0, bool autoRegisterGlobally = false)
        {
            OnEventNoArgs = onEventNoArgs;
            Priority = priority;
            
            if(autoRegisterGlobally)
                AutoRegisterGlobally();
        }

        private void AutoRegisterGlobally()
        {
            GlobalEventBus<T>.Register(this);
        }
    }
}