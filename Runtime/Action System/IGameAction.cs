using System;
using System.Collections.Generic;
using CustomTools.EventBusSystem;

namespace CustomTools.ActionSystem
{
    public interface IGameAction : IEvent
    {
        public object Triggerer { get; }
        public List<PerformData> PreReactions { get; }
        public List<PerformData> PerformReactions { get; }
        public List<PerformData> PostReactions { get; }
    }

    public class PerformData
    {
        public IGameAction action;
        public Action<IGameAction> callback;
    }
}