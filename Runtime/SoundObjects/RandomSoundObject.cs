using UnityEngine;

namespace CustomTools.SoundObjects
{
    [CreateAssetMenu(menuName = "Custom Tools/Audio/Random Sound Object")]
    public class RandomSoundObject : BaseSoundObjectSO
    {
        public SoundClip[] soundClips;

        protected override SoundClip GetClip()
        {
            int randomIndex = Random.Range(0, soundClips.Length);
            SoundClip clip = soundClips[randomIndex];
            return soundClips[randomIndex];
        }
    }
}