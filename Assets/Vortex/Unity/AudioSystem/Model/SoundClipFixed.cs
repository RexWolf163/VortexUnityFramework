using UnityEngine;

namespace Vortex.Unity.AudioSystem.Model
{
    /// <summary>
    /// Звуковой клип с зафиксированными значениями pitch и volume
    /// </summary>
    public class SoundClipFixed : SoundClip
    {
        /// <summary>
        /// Зафиксированное значение pitch
        /// </summary>
        private readonly float _currentPitch;

        /// <summary>
        /// Зафиксированное значение volume
        /// </summary>
        private readonly float _currentVolume;

        /// <summary>
        /// длительность клипа
        /// </summary>
        private readonly float _duration;

        public SoundClipFixed(SoundClip clip)
        {
            AudioClip = clip.AudioClip;
            _currentPitch = clip.GetPitch();
            _currentVolume = clip.GetVolume();
            _duration = _currentPitch == 0 ? float.MaxValue : clip.AudioClip.length / Mathf.Abs(_currentPitch);
        }

        public override float GetPitch() => _currentPitch;
        public override float GetVolume() => _currentVolume;

        public float GetDuration() => _duration;
    }
}