using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.LocalizationSystem;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Core.System.Enums;
using Vortex.Unity.LocalizationSystem;
using Vortex.Unity.UI.UIComponents.Attributes;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UI.UIComponents.Parts;

namespace Vortex.Unity.Components.Misc.LocalizationSystem
{
    /// <summary>
    /// Компонент для выставления фиксированного текста на UIComponent с возможностью локализации
    /// </summary>
    [RequireComponent(typeof(UIComponent)), ExecuteInEditMode]
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
            App.OnStart += RefreshData;
            RefreshData();
        }

        private void OnDisable()
        {
            Localization.OnLocalizationChanged -= RefreshData;
            App.OnStart -= RefreshData;
        }

        [Button("Set Locale")]
        private void RefreshData()
        {
            var state = App.GetState();
#if UNITY_EDITOR
            if (!Application.isPlaying)
                state = AppStates.Running;
#endif
            if (state < AppStates.Running)
                uiComponent.SetText("");
            else
                uiComponent.SetText(useLocalization ? key.Translate() : key);
        }

#if UNITY_EDITOR
        [OnInspectorInit]
        private void RefreshOnInspectorInit() => RefreshData();

        private void OnValidate()
        {
            if (uiComponent != null)
                return;
            uiComponent = gameObject.GetComponent<UIComponent>();
        }
#endif
    }
}