using UnityEngine;
using UnityEngine.Events;

namespace Vortex.UI.Components.UIComponents
{
    /// <summary>
    /// Структура данных для UI компонента
    /// </summary>
    public struct UIComponentData
    {
        public string[] texts;
        [HideInInspector] public UnityAction[] actions;
        public Sprite[] sprites;
    }
}