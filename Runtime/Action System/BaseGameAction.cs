using System.Collections.Generic;

namespace CustomTools.ActionSystem
{
    public abstract class BaseGameAction : IGameAction
    {
        public List<PerformData> PreReactions { get; } = new();
        public List<PerformData> PerformReactions { get; } = new();
        public List<PerformData> PostReactions { get; } = new();
    }
}