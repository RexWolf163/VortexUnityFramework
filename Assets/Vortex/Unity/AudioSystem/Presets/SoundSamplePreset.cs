using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;
using Random = UnityEngine.Random;

namespace Vortex.Unity.AudioSystem.Presets
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Database/SoundSample")]
    public class SoundSamplePreset : RecordPreset<Sound>
    {
        [SerializeField] private AudioClip audioClip;

        [SerializeField, MinMaxSlider(-3f, 3f)]
        private Vector2 pitchRange;

        [SerializeField, MinMaxSlider(0f, 1f)] private Vector2 valueRange = Vector2.one;

        public object Sample => new SoundClip(audioClip, pitchRange, valueRange);

#if UNITY_EDITOR
        private void OnValidate() => type = RecordTypes.Singleton;

        [Button]
        private void TestSound()
        {
            var pitch = Random.Range(pitchRange.x, pitchRange.y);
            var volume = Random.Range(valueRange.x, valueRange.y);
            var obj = new GameObject();
            var audio = obj.AddComponent<AudioSource>();
            audio.pitch = pitch;
            audio.PlayOneShot(audioClip, volume);
            var sd = obj.AddComponent<SelfDestroy>();
            sd.TargetTime = DateTime.Now.AddSeconds(audioClip.length / pitch);
        }

        [ExecuteInEditMode]
        private class SelfDestroy : MonoBehaviour
        {
            public DateTime TargetTime = DateTime.Now + new TimeSpan(0, 0, 10);

            private void Update()
            {
                if (DateTime.Now <= TargetTime)
                    return;
                if (!Application.isPlaying)
                    DestroyImmediate(gameObject);
                else
                    Destroy(gameObject);
            }
        }

#endif
    }
}