using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomTools.EventBusSystem
{
    public struct EventData<T>
    {
        public T eventToRaise;
        public Action<T> callback;
    }
    
    public class EventBus<T> where T : IEvent
    {
        private readonly List<IEventBinding<T>> _bindings = new List<IEventBinding<T>>();
        private List<IEventBinding<T>> _pendingRemoval = new List<IEventBinding<T>>();
        private List<IEventBinding<T>> _pendingAdd = new List<IEventBinding<T>>();
        private Queue<EventData<T>> _eventQueue = new Queue<EventData<T>>();
        
        private bool _eventInProgress;
        
        public void Register(EventBinding<T> binding)
        {
            _pendingAdd.Add(binding);
            
            if(!_eventInProgress)
                AddAllPending();
        }

        public void DeRegister(EventBinding<T> binding) => _pendingRemoval.Add(binding);//_bindings.Remove(binding);

        public void Raise(T eventToRaise, Action<T> callback = null)
        {
            EventData<T> eventData = new EventData<T>()
            {
                eventToRaise = eventToRaise,
                callback = callback
            };
            
            _eventQueue.Enqueue(eventData);
            
            if (!_eventInProgress)
            {
                RemoveAllPending();
                AddAllPending();
            }
            
            RaiseNextEvent();
        }

        private async void RaiseNextEvent()
        {
            if (_eventInProgress)
                return;

            if (_eventQueue.Count == 0)
                return;
            
            _eventInProgress = true;
            EventData<T> nextEventData = _eventQueue.Dequeue();
            bool callbackComplete = true;
            
            foreach (IEventBinding<T> binding in _bindings)
            {
                binding.OnEvent?.Invoke(nextEventData.eventToRaise);
                binding.OnEventNoArgs?.Invoke();

                if (binding.OnEventCallback == null) 
                    continue;
                
                callbackComplete = false;
                binding.OnEventCallback?.Invoke(nextEventData.eventToRaise, () =>
                {
                    callbackComplete = true;
                });
                    
                while(callbackComplete != true)
                    await Task.Yield();
            }

            nextEventData.callback?.Invoke(nextEventData.eventToRaise);
            
            RemoveAllPending();
            AddAllPending();
            
            RaiseComplete();
        }

        private void RaiseComplete()
        {
            _eventInProgress = false;
            RaiseNextEvent();
        }

        private void RemoveAllPending()
        {
            for (var i = 0; i < _pendingRemoval.Count; i++)
            {
                _bindings.Remove(_pendingRemoval[i]);
            }
            
            _pendingRemoval.Clear();
        }

        private void AddAllPending()
        {
            for (var i = 0; i < _pendingAdd.Count; i++)
            {
                _bindings.Add(_pendingAdd[i]);
            }
            
            _bindings.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            _pendingAdd.Clear();
        }
    }

    public static class GlobalEventBus<T> where T : IEvent
    {
        private static readonly EventBus<T> EventBus = new EventBus<T>();

        public static void Register(EventBinding<T> binding) => EventBus.Register(binding);
        public static void DeRegister(EventBinding<T> binding) => EventBus.DeRegister(binding);
        public static void Raise(T @event, Action<T> action = null) => EventBus.Raise(@event, action);
    }

    public interface ILocalEventBusSystem
    {
        public LocalEventBus EventBus { get; }
    }

    public class LocalEventBus
    {
        private readonly Dictionary<Type, object> _eventBuses = new Dictionary<Type, object>();

        public void Register<T>(EventBinding<T> binding) where T : IEvent
        {
            GetEventBus<T>().Register(binding);
        }

        public void DeRegister<T>(EventBinding<T> binding) where T : IEvent
        {
            GetEventBus<T>().DeRegister(binding);
        }
        
        public void Raise<T>(T eventToRaise, Action<T> callback = null, bool callGlobal = false) where T : IEvent
        {
            GetEventBus<T>().Raise(eventToRaise, (e) =>
            {
                if (!callGlobal)
                    callback?.Invoke(e);
                else
                    GlobalEventBus<T>.Raise(eventToRaise, callback);
            });
        }

        private EventBus<T> GetEventBus<T>() where T : IEvent
        {
            if (_eventBuses.TryGetValue(typeof(T), out var eventBus)) 
                return (EventBus<T>)eventBus;
            
            eventBus = new EventBus<T>();
            _eventBuses.Add(typeof(T), eventBus);

            return (EventBus<T>)eventBus;
        }
    }
}