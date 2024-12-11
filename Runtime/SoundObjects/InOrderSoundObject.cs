using UnityEngine;

namespace CustomTools.SoundObjects
{
    [CreateAssetMenu(menuName = "Custom Tools/Audio/In Order Sound Object")]
    public class InOrderSoundObject : BaseSoundObjectSO
    {
        public SoundClip[] soundClips;
        
        private int _currentIdx = 0;
        
        protected override SoundClip GetClip()
        {
            SoundClip clip = soundClips[_currentIdx];
            
            _currentIdx++;
            if (_currentIdx >= soundClips.Length)
                _currentIdx = 0;

            return clip;
        }
    }
}