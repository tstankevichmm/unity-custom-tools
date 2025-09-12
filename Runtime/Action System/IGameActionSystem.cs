using System;
using System.Threading.Tasks;
using CustomTools.Observer;

namespace CustomTools.ActionSystem
{
    public interface IGameActionSystem
    {
        public const int Start = -1000;
        public const int EarlyPre = -750;
        public const int Pre = -500;
        public const int LatePre = -250;
        public const int EarlyPost = 250;
        public const int Post = 500;
        public const int LatePost = 750;
        public const int Finish = 1000;
    
        public ObserverValue<bool> IsPerforming { get; }
        public void Perform(IGameAction action, Action<IGameAction> callback = null);
        public void SetPerformer<T>(Func<T, Task> newPerformer) where T : IGameAction;
        public void UnsetPerformer<T>() where T : IGameAction;
        public void SubscribeToPerformer<T>(Func<IGameAction, Task> newSubscription, int timing) where T : IGameAction;
        public void UnsubscribeFromPerformer<T>(Func<IGameAction, Task> subscription) where T : IGameAction;
    }
    
    public struct SubscriptionData
    {
        public Type type;
        public Func<IGameAction, Task> func;
        public int priority;
    }
}