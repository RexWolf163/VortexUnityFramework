using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.UI.UIComponents.Attributes;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UI.UIComponents.Parts;

namespace Vortex.Unity.Components.Misc.LocalizationSystem
{
    /// <summary>
    /// Компонент для выставления фиксированного спрайта на UIComponent
    /// </summary>
    [RequireComponent(typeof(UIComponent))]
    public class SetSpriteComponent : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;

        [SerializeField] private UIComponent uiComponent;

        [SerializeField, UIComponentLink(typeof(UIComponentGraphic), "uiComponent"), TitleGroup("Link")]
        private int position;

        private void OnEnable()
        {
            if (uiComponent == null)
            {
                Debug.LogError($"Target for SetText:{name} component is missing.");
                return;
            }

            RefreshData();
        }

        private void OnDisable()
        {
        }

        [Button("Set Sprite")]
        private void RefreshData() => uiComponent.SetSprite(sprite);

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