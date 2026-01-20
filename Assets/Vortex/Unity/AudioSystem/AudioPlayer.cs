using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.Extensions.Abstractions;
using Vortex.Unity.UI.PoolSystem;

namespace Vortex.Unity.AudioSystem
{
    public class AudioPlayer : MonoBehaviourSingleton<AudioPlayer>
    {
        [SerializeField] private Pool pool;

        public static void Play(Sound sound)
        {
            if (Instance == null)
                return;
            var clip = new SoundClipFixed(sound.Sample);
            Instance.pool.AddItem(clip);
            TimeController.Call(() => Instance.pool.RemoveItem(clip), clip.GetDuration(), clip);
        }
    }
}