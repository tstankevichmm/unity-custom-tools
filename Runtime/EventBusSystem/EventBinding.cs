using System;

namespace CustomTools.EventBusSystem
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        public int Priority { get; set; }
        public Action<T, Action> OnEventCallback { get; set; }
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }

        public EventBinding(Action<T, Action> onEventCallback, int priority = 0)
        {
            OnEventCallback = onEventCallback;
            Priority = priority;
        }

        public EventBinding(Action<T> onEvent, int priority = 0)
        {
            OnEvent = onEvent;
            Priority = priority;
        }

        public EventBinding(Action onEventNoArgs, int priority = 0)
        {
            OnEventNoArgs = onEventNoArgs;
            Priority = priority;
        }
    }
}