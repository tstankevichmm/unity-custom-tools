using UnityEngine.Audio;

namespace CustomTools.SoundObjects
{
    public interface ISoundObject
    {
        public AudioMixerGroup AudioMixerGroup { get; }
        public SoundClip GetClip(out float preferredVolume);
    }
}

