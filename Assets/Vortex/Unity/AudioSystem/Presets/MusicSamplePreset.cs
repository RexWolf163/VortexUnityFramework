using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.AudioSystem.Presets
{
    [CreateAssetMenu(fileName = "MusicSample", menuName = "Database/MusicSample")]
    public class MusicSamplePreset : RecordPreset<Music>
    {
        [SerializeField] private AudioClip audioClip;

        [SerializeField, Range(-3f, 3f)] private float pitchRange = 1f;

        [SerializeField, Range(0f, 1f)] private float valueRange = 1f;

        public SoundClip Sample =>
            new(audioClip, new Vector2(pitchRange, pitchRange), new Vector2(valueRange, valueRange));

        public float Duration => pitchRange == 0 ? float.MaxValue : audioClip.length / Mathf.Abs(pitchRange);

#if UNITY_EDITOR
        private GameObject _testObject;

        private bool IsPlay() => _testObject != null;

        private void OnValidate() => type = RecordTypes.Singleton;

        [Button, HideIf("IsPlay")]
        private void TestSound()
        {
            var pitch = pitchRange == 0 ? 0.1f : Mathf.Abs(pitchRange);
            var volume = valueRange;
            _testObject = new GameObject();
            var audio = _testObject.AddComponent<AudioSource>();
            audio.pitch = pitch;
            audio.PlayOneShot(audioClip, volume);
            var sd = _testObject.AddComponent<SelfDestroy>();
            sd.TargetTime = DateTime.Now.AddSeconds(audioClip.length / pitch);
        }

        [Button, HideIf("@!IsPlay()")]
        private void StopSound()
        {
            if (Application.isPlaying)
                Destroy(_testObject);
            else
                DestroyImmediate(_testObject);

            _testObject = null;
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