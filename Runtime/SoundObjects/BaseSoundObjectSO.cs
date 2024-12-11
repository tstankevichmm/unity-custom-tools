using UnityEngine;
using UnityEngine.Audio;

namespace CustomTools.SoundObjects
{
    public abstract class BaseSoundObjectSO : ScriptableObject, ISoundObject
    {
        [field:SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }
        public float defaultVolume = 1f;
        
        public SoundClip GetClip(out float preferredVolume)
        {
            SoundClip clip = GetClip();
            preferredVolume = clip.overrideDefaultVolume ? clip.overriddenVolume : defaultVolume;
            return clip;
        }

        protected abstract SoundClip GetClip();
    }
}