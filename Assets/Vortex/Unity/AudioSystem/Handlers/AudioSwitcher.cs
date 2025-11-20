using UnityEngine;
using UnityEngine.UI;
using Vortex.Core.AudioSystem;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.Extensions.DefaultEnums;
using Vortex.Unity.UI.Attributes;
using Vortex.Unity.UI.StateSwitcher;

namespace Vortex.Unity.AudioSystem.Handlers
{
    public class AudioSwitcher : MonoBehaviour
    {
        [SerializeField, StateSwitcher(typeof(SwitcherState))]
        private UIStateSwitcher switcher;

        [SerializeField] private Button button;
        [SerializeField] private SoundType controlType;

        private void OnEnable()
        {
            button.onClick.AddListener(OnChange);
            Refresh();
        }

        private void OnChange()
        {
            if (controlType == SoundType.Sound)
                AudioProvider.SetSoundState(!AudioProvider.Settings.SoundOn);
            else
                AudioProvider.SetMusicState(!AudioProvider.Settings.MusicOn);
            Refresh();
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnChange);
        }

        private void Refresh()
        {
            var state = controlType == SoundType.Sound
                ? AudioProvider.Settings.SoundOn
                : AudioProvider.Settings.MusicOn;
            switcher.Set(state ? SwitcherState.On : SwitcherState.Off);
        }
    }
}