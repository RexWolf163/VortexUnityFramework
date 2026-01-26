#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    public abstract class UIComponentPart : MonoBehaviour
    {
#if UNITY_EDITOR
        [OnInspectorInit]
        protected void OnInspector()
        {
            var rect = transform.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchorMin = Vector2.zero;
        }
#endif
    }
}