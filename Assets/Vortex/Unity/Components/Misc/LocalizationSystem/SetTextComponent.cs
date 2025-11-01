using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.UIComponents.Attributes;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UI.UIComponents.Parts;

namespace Vortex.Unity.Components.Misc.LocalizationSystem
{
    [RequireComponent(typeof(UIComponent))]
    public class SetTextComponent : MonoBehaviour
    {
        [SerializeField, LocalizationKey] private string key;
        [SerializeField] private bool useLocalization = true;

        [SerializeField] private UIComponent uiComponent;

        [SerializeField, UIComponentLink(typeof(UIComponentText), "uiComponent"), TitleGroup("Link")]
        private int position;

        private void OnEnable()
        {
            if (uiComponent == null)
            {
                Debug.LogError($"Target for SetText:{name} component is missing.");
                return;
            }

            Localization.OnLocalizationChanged += RefreshData;
            RefreshData();
        }

        private void OnDisable()
        {
            Localization.OnLocalizationChanged -= RefreshData;
        }

        [Button("Set Locale")]
        private void RefreshData() => uiComponent.SetText(useLocalization ? key.Translate() : key);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (uiComponent != null)
                return;
            uiComponent = gameObject.GetComponent<UIComponent>();
        }
#endif
    }
}