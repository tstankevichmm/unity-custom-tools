using CustomTools.UnityExtensions;
using UnityEngine;

namespace CustomTools.AnimationEvents
{
    public class ObjectSpawnAnimationEventStateBehavior : AnimationEventStateBehaviour
    {
        [SerializeField] private GameObject _objectToSpawn;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private bool _shouldParent;
        
        protected override void NotifyReceiver(Animator animator)
        {
            base.NotifyReceiver(animator);
            
            Transform goTransform = animator.gameObject.transform;
            GameObject newGO = Instantiate(_objectToSpawn, goTransform.position + goTransform.GetOffset(_offset), goTransform.rotation);
            
            if(_shouldParent)
                newGO.transform.SetParent(goTransform);
        }
    }
}