using System;
using CustomTools.EventBusSystem;
using UnityEngine.Events;

namespace CustomTools.AnimationEvents
{
    [Serializable]
    public class AnimationSignal
    {
        public string eventName;
        public UnityEvent OnAnimationEvent;
    }

    public class AnimationSignalEvent : IEvent
    {
        public string eventName;
    }
}