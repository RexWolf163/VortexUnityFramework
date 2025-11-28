using UnityEngine;

namespace Vortex.Unity.AudioSystem.Model
{
    /// <summary>
    /// Звуковой клип
    /// Содержит диапазоны допустимых pitch и volume
    /// </summary>
    public class SoundClip
    {
        public AudioClip AudioClip { get; protected set; }
        public Vector2 PitchRange { get; }
        public Vector2 ValueRange { get; }

        public SoundClip(AudioClip audioClip, Vector2 pitchRange, Vector2 valueRange)
        {
            AudioClip = audioClip;
            PitchRange = pitchRange;
            ValueRange = valueRange;
        }

        protected SoundClip()
        {
        }

        public virtual float GetPitch() => Random.Range(PitchRange.x, PitchRange.y);
        public virtual float GetVolume() => Random.Range(PitchRange.x, PitchRange.y);
    }
}