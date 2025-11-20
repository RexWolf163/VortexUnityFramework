using UnityEngine;
using UnityEngine.UI;
using Vortex.Core.AudioSystem;
using Vortex.Core.AudioSystem.Bus;

namespace Vortex.Unity.AudioSystem.Handlers
{
    public class AudioValueSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [SerializeField] private SoundType controlType;

        private void OnEnable()
        {
            slider.value = controlType == SoundType.Sound
                ? AudioProvider.Settings.SoundVolume
                : AudioProvider.Settings.MusicVolume;

            slider.onValueChanged.AddListener(OnChange);
        }

        private void OnChange(float value)
        {
            if (controlType == SoundType.Sound)
                AudioProvider.SetSoundVolume(value);
            else
                AudioProvider.SetMusicVolume(value);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnChange);
        }
    }
}