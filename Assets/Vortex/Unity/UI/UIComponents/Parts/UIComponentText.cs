using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    public class UIComponentText : UIComponentPart
    {
        [InfoBox("Можно выбрать один из вариантов или все сразу")]
        [SerializeField] protected Text textField;

        [SerializeField] protected TextMeshPro textMPField;
        [SerializeField] protected TextMeshProUGUI textMPUiField;

#if UNITY_EDITOR
        [OnInspectorInit]
        private void Search()
        {
            if (textField != null)
                return;
            textField = GetComponent<Text>();
        }
#endif

        public virtual void PutData(string text)
        {
            SetText(text);
        }

        protected void SetText(string value)
        {
            if (textField != null)
                textField.text = value;
            if (textMPField != null)
                textMPField.text = value;
            if (textMPUiField != null)
                textMPUiField.text = value;
        }

        protected void AppendChar(char c)
        {
            if (textField != null)
                textField.text += c;
            if (textMPField != null)
                textMPField.text += c;
            if (textMPUiField != null)
                textMPUiField.text += c;
        }

        public string GetValue()
        {
            if (textField != null)
                return textField.text;
            if (textMPField != null)
                return textMPField.text;
            if (textMPUiField != null)
                return textMPUiField.text;
            return String.Empty;
        }

        protected virtual void OnDestroy()
        {
            SetText(String.Empty);
        }
    }
}