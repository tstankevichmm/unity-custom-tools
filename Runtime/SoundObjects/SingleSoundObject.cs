using UnityEngine;

namespace CustomTools.SoundObjects
{
    [CreateAssetMenu(menuName = "Custom Tools/Audio/Single Sound Object")]
    public class SingleSoundObject : BaseSoundObjectSO
    {
        public SoundClip soundClip;
       
        protected override SoundClip GetClip()
        {
            return soundClip;
        }
    }
}