using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomTools.Observer;
using UnityEngine;

namespace CustomTools.ActionSystem
{
    public class GameActionSystem : IGameActionSystem
    {
        public ObserverValue<bool> IsPerforming { get; private set; } = new(false);

        private readonly Dictionary<Type, List<SubscriptionData>> _subscriptions = new();
        private readonly List<SubscriptionData> _pendingSubRemoval = new();
        private readonly Dictionary<Type, Func<IGameAction, Task>> _performers = new();
        
        private List<PerformData> _reactions = null;
        private IGameAction _currentGameAction;
        private bool _shouldLog;

        public async void Perform(IGameAction action, Action<IGameAction> callback = null) 
        {
            Log($"GameActionSystem.Perform: Perform action of type {action.GetType()}");
            
            if (IsPerforming)
            {
                Log($"GameActionSystem.Perform: Already performing action {_currentGameAction.GetType()}, will perform {action.GetType()} as a reaction.");
                
                PerformData reactionPerformData = new PerformData()
                {
                    action = action,
                    callback = callback
                };
                
                await ActionFlow(reactionPerformData);
                reactionPerformData.callback?.Invoke(reactionPerformData.action);
                return;
            }
            
            IsPerforming.Value = true;

            PerformData data = new PerformData()
            {
                action = action,
                callback = callback
            };
            
            Log($"GameActionSystem.ResolveQueue: Resolve action of type {data.action.GetType()}");
                
            await ActionFlow(data);
                
            IsPerforming.Value = false;
            data.callback?.Invoke(data?.action);
        }

        public void SetShouldLog(bool shouldLog)
        {
            _shouldLog = shouldLog;
        }
        
        private async Task PerformReactions()
        {
            Log($"GameActionSystem.PerformReactions: Perform {_reactions.Count} Reactions");
            foreach (PerformData data in _reactions)
            {
                Log($"\t{data.action.GetType()}");
            }
            
            foreach (PerformData data in _reactions)
            {
                await ActionFlow(data);
                data.callback?.Invoke(data.action);
            }
            
            Log("GameActionSystem.PerformReactions: Perform Reactions Complete");
        }

        private async Task ActionFlow(PerformData performData)
        {
            IGameAction gameAction = performData.action;
            
            _currentGameAction = gameAction;
            Type type = gameAction.GetType();
            Log($"GameActionSystem.ActionFlow: {type}");
            
            _reactions = gameAction.PreReactions;

            if (_subscriptions.ContainsKey(type))
            {
                Log("GameActionSystem.ActionFlow: Perform Pre Subscribers");
                List<SubscriptionData> preSubs = _subscriptions[gameAction.GetType()].Where(x => x.priority < 0).ToList();
                await PerformSubscribers(gameAction, preSubs);
            }

            await PerformReactions();

            Log("GameActionSystem.ActionFlow: Perform");
            _reactions = gameAction.PerformReactions;
            await PerformPerformer(gameAction);
            await PerformReactions();

            _reactions = gameAction.PostReactions;

            if (_subscriptions.ContainsKey(type))
            {
                Log("GameActionSystem.ActionFlow: Perform Post Subscribers");
                List<SubscriptionData> postSubs = _subscriptions[gameAction.GetType()].Where(x => x.priority >= 0).ToList();
                await PerformSubscribers(gameAction, postSubs);
            }

            await PerformReactions();
        }

        private async Task PerformPerformer(IGameAction gameAction)
        {
            Type type = gameAction.GetType();
            
            if(_performers.ContainsKey(type))
                await _performers[type](gameAction);
        }

        private async Task PerformSubscribers(IGameAction gameAction, List<SubscriptionData> subDataList)
        {
            subDataList.Sort((x, y) => x.priority.CompareTo(y.priority));
            
            foreach (SubscriptionData subData in subDataList)
            {
                await subData.func(gameAction);
            }

            RemovePendingSubs();
        }

        private void RemovePendingSubs()
        {
            foreach (SubscriptionData removalData in _pendingSubRemoval)
            {
                if (_subscriptions.TryGetValue(removalData.type, out var sub))
                    sub.RemoveAll(x => x.func == removalData.func);
            }
            
            _pendingSubRemoval.Clear();
        }

        public void SetPerformer<T>(Func<T, Task> newPerformer) where T : IGameAction
        {
            Type type = typeof(T);
            
            Task wrappedPerformer(IGameAction action) => newPerformer((T)action);
            
            if (_performers.ContainsKey(type))
                _performers[type] = wrappedPerformer;
            else 
                _performers.Add(type, wrappedPerformer);
        }

        public void UnsetPerformer<T>() where T : IGameAction
        {
            Type type = typeof(T);

            if (_performers.ContainsKey(type))
                _performers.Remove(type);
        }

        public void SubscribeToPerformer<T>(Func<IGameAction, Task> newSubscription, int timing) where T : IGameAction
        {
            Type type = typeof(T);
            
            SubscriptionData subData = new SubscriptionData()
            {
                type = type,
                func = newSubscription,
                priority = timing
            };
            
            if (_subscriptions.ContainsKey(type))
            {
                _subscriptions[type].Add(subData);
                _subscriptions[type].Sort((x, y) => x.priority.CompareTo(y.priority));
            }
            else
            {
                _subscriptions.Add(type, new List<SubscriptionData> { subData });
            }
        }

        public void UnsubscribeFromPerformer<T>(Func<IGameAction, Task> subscription) where T : IGameAction
        {
            Type type = typeof(T);
            
            SubscriptionData removalData = new SubscriptionData()
            {
                type = type,
                func = subscription
            };

            if (_subscriptions.ContainsKey(type))
                _pendingSubRemoval.Add(removalData);
        }

        private void AddReaction(IGameAction gameAction, Action<IGameAction> callback = null)
        {
            _reactions ??= new List<PerformData>();

            PerformData data = new PerformData()
            {
                action = gameAction,
                callback = callback
            };
            
            _reactions.Add(data);
        }

        private void Log(string message)
        {
            if (!_shouldLog)
                return;
            
            Debug.Log(message);
        }
    }
}