using UnityEngine;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.UIComponents;

namespace AppScripts.LocalizationSystem
{
    public class SetLocaleHandler : MonoBehaviour
    {
        [SerializeField] private UIComponent uiComponent;

        [SerializeField, Language] private SystemLanguage language;

        [SerializeField] private bool useSwitch;

        private void Awake()
        {
            uiComponent.SetAction(Run);
            Localization.OnInit += Refresh;
        }

        public void Run()
        {
            if (!isActiveAndEnabled)
                return;
            Localization.SetCurrentLanguage(language);
            Refresh();
        }

        private void Refresh()
        {
            uiComponent.SetText(language.ToString());
            if (!useSwitch)
                return;
            uiComponent.SetSwitcher(Localization.GetCurrentLanguage() == language ? 0 : 1);
        }

        private void OnEnable()
        {
            Localization.OnLocalizationChanged += Refresh;
        }

        private void OnDisable()
        {
            Localization.OnLocalizationChanged -= Refresh;
        }
    }
}