using CustomTools.EventBusSystem;
using UnityEngine;

namespace CustomTools.AnimationEvents
{
    public class AnimationEventStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string _eventName;
        [Range(0f, 1f), SerializeField] private float _triggerTime;
        [SerializeField] private bool _triggerEveryLoop;
        
        private bool _hasTriggered;

        public float GetTriggerTime()
        {
            return _triggerTime;
        }
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasTriggered = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float currentTime = stateInfo.normalizedTime % 1;

            if (!_hasTriggered && currentTime >= _triggerTime)
            {
                NotifyReceiver(animator);
                _hasTriggered = true;
            }
            else if ((_triggerEveryLoop && currentTime < _triggerTime) || _triggerTime == 0 && currentTime == 0)
            {
                _hasTriggered = false;
            }
        }

        protected virtual void NotifyReceiver(Animator animator)
        {
            NotifyEventReceiver(animator);
            NotifyLocalEventBus(animator);
            NotifyGlobalEventBus(animator);
        }

        private void NotifyGlobalEventBus(Animator animator)
        {
            AnimationSignalEvent signalEvent = new AnimationSignalEvent()
            {
                eventName = _eventName
            };
            
            GlobalEventBus<AnimationSignalEvent>.Raise(signalEvent);
        }

        private void NotifyLocalEventBus(Animator animator)
        {
            ILocalEventBusSystem localEventBusSystem = animator.GetComponent<ILocalEventBusSystem>();

            if (localEventBusSystem == null)
                return;

            AnimationSignalEvent signalEvent = new AnimationSignalEvent()
            {
                eventName = _eventName
            };
            
            localEventBusSystem.EventBus.Raise(signalEvent);
        }

        private void NotifyEventReceiver(Animator animator)
        {
            AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();

            if (receiver == null)
                return;
            
            receiver.OnAnimationEventTriggered(_eventName);
        }
    }
}