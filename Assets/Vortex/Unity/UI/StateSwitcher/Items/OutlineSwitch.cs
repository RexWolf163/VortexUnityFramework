using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.UI.Components.StateSwitcher.Items
{
    public class OutlineSwitch : StateItem
    {
        [SerializeField] [HorizontalGroup("h1")]  [HorizontalGroup("h1", 70f)] private Outline _outline;
        [SerializeField]  [HorizontalGroup("h1")][HideLabel]  private Color _color;

        public override void Set()
        {
            _outline.effectColor = _color;
        }

        public override void DefaultState()
        {
            _outline.effectColor = Color.clear;
        }

#if UNITY_EDITOR
        [ShowIf("@_outline != null")]
        [Button("Get from...")]
        [GUIColor("@Color.green")]
        [HorizontalGroup("h1")]
        private void SetCurrent()
        {
            _color = _outline.effectColor;
        }
        public override StateItem Clone()
        {
            return new OutlineSwitch()
            {
                _outline = _outline,
                _color = _color
            };
        }

        public override string DropDownItemName => "Graphics";
        public override string DropDownGroupName => "Outlice Switch";
#endif
    }
}