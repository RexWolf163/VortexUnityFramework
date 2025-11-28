using System;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.UI.PoolSystem;

namespace Vortex.Unity.AudioSystem
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private Pool pool;

        private static AudioPlayer _instance;

        private void OnEnable()
        {
            _instance = this;
        }

        private void OnDisable()
        {
            _instance = null;
        }

        public static void Play(Sound sound)
        {
            if (_instance == null)
                return;
            var clip = new SoundClipFixed(sound.Sample);
            _instance.pool.AddItem(clip);
            TimeController.Call(() => _instance.pool.RemoveItem(clip), clip.GetDuration(), clip);
        }
    }
}