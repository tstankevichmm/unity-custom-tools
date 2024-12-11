using UnityEngine;

namespace CustomTools.SoundObjects
{
    [CreateAssetMenu(menuName = "Custom Tools/Audio/Reverse Order Sound Object")]
    public class ReverseOrderSoundObject : BaseSoundObjectSO
    {
        public SoundClip[] soundClips;
        
        private int _currentIdx = 0;
        
        protected override SoundClip GetClip()
        {
            _currentIdx--;

            if (_currentIdx < 0)
                _currentIdx = soundClips.Length - 1;
            
            SoundClip clip = soundClips[_currentIdx];
            return clip;
        }
    }
}