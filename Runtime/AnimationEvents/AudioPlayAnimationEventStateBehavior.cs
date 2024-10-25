using UnityEngine;

namespace CustomTools.AnimationEvents
{
    public class AudioPlayAnimationEventStateBehavior : AnimationEventStateBehaviour
    {
        [SerializeField] private AudioClip _audioClip;

        protected override void NotifyReceiver(Animator animator)
        {
            base.NotifyReceiver(animator);
            PlayAudioClipFromGameObject(animator.gameObject, _audioClip);
        }

        private void PlayAudioClipFromGameObject(GameObject go, AudioClip audioClip)
        {
            AudioSource audioSource = go.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning($"Could not find an audio source on {go.name}");
                return;
            }
            
            audioSource.PlayOneShot(audioClip);
        }
    }
}