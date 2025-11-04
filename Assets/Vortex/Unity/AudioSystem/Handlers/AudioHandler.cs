using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.AudioSystem.Model;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.AudioSystem.Model;
using Vortex.Unity.DatabaseSystem.Attributes;

namespace Vortex.Unity.AudioSystem.Handlers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private bool isMusic = false;

        [SerializeField, DbRecord(typeof(AudioSample))]
        private string audioSample;

        private SoundClip _sound;
        private float _currentVolume;
        private float _currentPitch;


        private void Awake()
        {
            AudioProvider.OnInit += OnInit;
        }

        private void OnDestroy()
        {
            AudioProvider.OnInit -= OnInit;
            TimeController.RemoveCall(this);
        }

        private void OnInit()
        {
            TimeController.Accumulate(Init, this);
        }


        private void Init()
        {
            var sample = AudioProvider.GetSample(audioSample);
            switch (sample)
            {
                case Sound snd:
                    isMusic = false;
                    _sound = snd.GetData();
                    break;
                case MusicSample music:
                    audioSource.clip = music.Sample as AudioClip;
                    isMusic = true;
                    if (isActiveAndEnabled)
                        Play();
                    break;
                default:
                    Debug.LogError($"AudioHandler::Awake: Unknown sample type: {sample}");
                    break;
            }
        }

        private void OnEnable()
        {
            AudioProvider.OnSettingsChanged += CheckSettings;
            if (audioSource.clip == null)
                return;
            CheckSettings();
            if (isMusic)
                Play();
        }

        private void OnDisable()
        {
            AudioProvider.OnSettingsChanged -= CheckSettings;
            Stop();
        }

        private void CheckSettings()
        {
            if (isMusic)
            {
                audioSource.mute = !AudioProvider.Settings.MusicOn;
                audioSource.volume = AudioProvider.Settings.MusicVolume * _currentVolume;
                audioSource.pitch = _currentPitch;
            }
            else
            {
                audioSource.mute = !AudioProvider.Settings.SoundOn;
                audioSource.volume = AudioProvider.Settings.SoundVolume * _currentVolume;
                audioSource.pitch = _currentPitch;
            }
        }

        [HorizontalGroup("h1"), Button]
        public void Play()
        {
            _currentPitch = isMusic ? 1f : _sound.GetPitch();
            _currentVolume = isMusic ? 1f : _sound.GetVolume();
            CheckSettings();
            if (isMusic)
                audioSource.Play();
            else
                audioSource.PlayOneShot(_sound.AudioClip);
        }

        [HorizontalGroup("h1"), Button]
        public void Stop() => audioSource.Stop();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }
#endif
    }
}