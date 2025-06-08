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
        
        public void Register(IEventBinding<T> binding)
        {
            _pendingAdd.Add(binding);
            
            if(!_eventInProgress)
                AddAllPending();
        }

        public List<IEventBinding<T>> GetBindings()
        {
            return new List<IEventBinding<T>>(_bindings);
        }

        public void DeRegister(IEventBinding<T> binding) => _pendingRemoval.Add(binding);//_bindings.Remove(binding);

        public async Task Raise(T eventToRaise, Action<T> callback = null)
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
            
            await RaiseNextEvent();
        }

        private async Task RaiseNextEvent()
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

                if (binding.OnEventCallback != null)
                {
                    callbackComplete = false;
                    binding.OnEventCallback?.Invoke(nextEventData.eventToRaise, () =>
                    {
                        callbackComplete = true;
                    });

                    while(callbackComplete != true)
                        await Task.Yield();
                }

                if(binding.OnEventAsync == null)
                    continue;
                
                await binding.OnEventAsync.Invoke(nextEventData.eventToRaise);
            }

            nextEventData.callback?.Invoke(nextEventData.eventToRaise);
            
            RemoveAllPending();
            AddAllPending();
            
            await RaiseComplete();
        }

        private async Task RaiseComplete()
        {
            _eventInProgress = false;
            await RaiseNextEvent();
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

        public static List<IEventBinding<T>> GetBindings() => EventBus.GetBindings();
        public static void Register(IEventBinding<T> binding) => EventBus.Register(binding);
        public static void DeRegister(IEventBinding<T> binding) => EventBus.DeRegister(binding);
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