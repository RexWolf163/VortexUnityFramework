using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    public class UIComponentText : UIComponentPart
    {
        [SerializeField] private TextMeshProUGUI textField;

#if UNITY_EDITOR
        [OnInspectorInit]
        private void Search()
        {
            if (textField != null)
                return;
            textField = GetComponent<TextMeshProUGUI>();
        }
#endif
        public void PutData(string text) => textField.text = text;

        private void OnDestroy() => textField.text = "";

        public string GetValue() => textField.text;
    }
}