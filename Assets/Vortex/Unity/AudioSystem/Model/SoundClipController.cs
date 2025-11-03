using UnityEngine;

namespace Vortex.Unity.AudioSystem.Model
{
    public static class SoundClipController
    {
        public static void PlayOnSource(this Sound sound, AudioSource audioSource)
        {
            var clip = sound.GetData();
            audioSource.clip = clip.AudioClip;
            audioSource.pitch = clip.GetPitch();
            audioSource.volume = clip.GetVolume();
            audioSource.Play();
        }
    }
}