using System.Collections.Generic;
using UnityEngine;

namespace CustomTools.AnimationEvents
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        [SerializeField] private List<AnimationSignal> _animationEvents;
        public void OnAnimationEventTriggered(string eventName)
        {
            AnimationSignal animationSignal = GetAnimationEvent(eventName);
            animationSignal?.OnAnimationEvent?.Invoke();
        }

        private AnimationSignal GetAnimationEvent(string eventName)
        {
            return _animationEvents.Find(animEvent => animEvent.eventName == eventName);
        }
    }
}