using System;

namespace CustomTools.EventBusSystem
{
    public interface IEventBinding<T>
    {
        public int Priority { get; set; }
        public Action<T, Action> OnEventCallback { get; set; }
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
}