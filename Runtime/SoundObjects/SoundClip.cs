using UnityEngine;

namespace CustomTools.SoundObjects
{
    [System.Serializable]
    public struct SoundClip
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float startTime;
        public bool overrideDefaultVolume;
        public float overriddenVolume;
    }
}