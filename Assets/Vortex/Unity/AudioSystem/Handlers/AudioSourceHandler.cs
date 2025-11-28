using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Unity.AudioSystem.Model;

namespace Vortex.Unity.AudioSystem.Handlers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject dataStorageObject;

        private IDataStorage _dataStorage;

        private SoundClip _sound;

        private float _currentVolume;
        private float _currentPitch;

        private void CheckSettings()
        {
            audioSource.mute = !AudioProvider.Settings.SoundOn;
            audioSource.volume = AudioProvider.Settings.SoundVolume * _currentVolume;
            audioSource.pitch = _currentPitch;
        }

        private void Awake() => _dataStorage = dataStorageObject.GetComponent<IDataStorage>();

        private void OnEnable() => Play();

        private void OnDisable() => Stop();

        [HorizontalGroup("h1"), Button, ShowIf("ShowBtns")]
        private void Play()
        {
            _sound = _dataStorage.GetData<SoundClip>();
            if (_sound == null)
                return;
            _currentPitch = _sound.GetPitch();
            _currentVolume = _sound.GetVolume();
            CheckSettings();
            audioSource.PlayOneShot(_sound.AudioClip);
        }

        [HorizontalGroup("h1"), Button, ShowIf("ShowBtns")]
        private void Stop() => audioSource.Stop();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            if (dataStorageObject == null)
                return;
            _dataStorage = dataStorageObject.GetComponent<IDataStorage>();

            if (_dataStorage == null)
                dataStorageObject = null;
        }

        private bool ShowBtns() => Application.isPlaying;

#endif
    }
}