using System;
using System.Threading.Tasks;

namespace CustomTools.EventBusSystem
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        public string BindingID { get; private set; }
        public int Priority { get; private set; }
        public Action<T> OnEvent { get; private set; }
        public Action OnEventNoArgs { get; private set; }
        public Func<T, Task> OnEventAsync { get; private set; }
        
        private EventBinding()
        {
            BindingID = Guid.NewGuid().ToString();
        }

        public class Builder
        {
            private int _priority = 0;
            private Action<T> _onEvent;
            private Action _onEventNoArgs;
            private Func<T, Task> _onEventAsync;
            private bool _registerGlobally;
            
            public Builder WithPriority(int priority)
            {
                _priority = priority;
                return this;
            }
            
            public Builder WithOnEvent(Action<T> onEvent)
            {
                _onEvent = onEvent;
                return this;
            }
            
            public Builder WithOnEventNoArgs(Action onEventNoArgs)
            {
                _onEventNoArgs = onEventNoArgs;
                return this;
            }
            
            public Builder WithOnEventAsync(Func<T, Task> onEventAsync)
            {
                _onEventAsync = onEventAsync;
                return this;
            }

            public Builder WithGlobalRegister()
            {
                _registerGlobally = true;
                return this;
            }

            public EventBinding<T> Build()
            {
                EventBinding<T> binding = new EventBinding<T>
                {
                    Priority = _priority,
                    OnEvent = _onEvent,
                    OnEventNoArgs = _onEventNoArgs,
                    OnEventAsync = _onEventAsync
                };
                
                if(_registerGlobally)
                    binding.RegisterGlobally();
                
                return binding;
            }
            
        }
        
        public void DeRegisterGlobally()
        {
            GlobalEventBus<T>.DeRegister(this);
        }

        public void RegisterGlobally()
        {
            GlobalEventBus<T>.Register(this);
        }
    }
}