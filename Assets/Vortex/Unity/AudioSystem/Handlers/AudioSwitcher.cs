using UnityEngine;
using Vortex.Core.AudioSystem;
using Vortex.Core.AudioSystem.Bus;
using Vortex.Core.Extensions.DefaultEnums;
using Vortex.Unity.UI.UIComponents;

namespace Vortex.Unity.AudioSystem.Handlers
{
    public class AudioSwitcher : MonoBehaviour
    {
        [SerializeField] private UIComponent uiComponent;
        [SerializeField] private SoundType controlType;

        private void OnEnable()
        {
            uiComponent.SetAction(OnChange);
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
        }

        private void Refresh()
        {
            var state = controlType == SoundType.Sound
                ? AudioProvider.Settings.SoundOn
                : AudioProvider.Settings.MusicOn;
            uiComponent.SetSwitcher(state ? SwitcherState.On : SwitcherState.Off);
        }
    }
}