using UnityEngine;

namespace Vortex.Unity.AudioSystem.Model
{
    public class SoundClip
    {
        public AudioClip AudioClip { get; }
        public Vector2 PitchRange { get; }
        public Vector2 ValueRange { get; }

        public SoundClip(AudioClip audioClip, Vector2 pitchRange, Vector2 valueRange)
        {
            AudioClip = audioClip;
            PitchRange = pitchRange;
            ValueRange = valueRange;
        }

        public float GetPitch() => Random.Range(PitchRange.x, PitchRange.y);
        public float GetVolume() => Random.Range(PitchRange.x, PitchRange.y);
    }
}