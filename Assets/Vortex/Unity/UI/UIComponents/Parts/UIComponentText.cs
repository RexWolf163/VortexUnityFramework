using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    public class UIComponentText : UIComponentPart
    {
        [SerializeField] private Text textField;

#if UNITY_EDITOR
        [OnInspectorInit]
        private void Search()
        {
            if (textField != null)
                return;
            textField = GetComponent<Text>();
        }
#endif
        public void PutData(string text) => textField.text = text;

        private void OnDestroy() => textField.text = "";

        public string GetValue() => textField.text;
    }
}