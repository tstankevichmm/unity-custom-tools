using System.Collections.Generic;

namespace CustomTools.ActionSystem
{
    public abstract class BaseGameAction : IGameAction
    {
        public object Triggerer { get; protected set; }
        public List<PerformData> PreReactions { get; } = new();
        public List<PerformData> PerformReactions { get; } = new();
        public List<PerformData> PostReactions { get; } = new();
    }
}