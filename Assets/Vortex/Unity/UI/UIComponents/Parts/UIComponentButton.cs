using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Vortex.UI.Components.UIComponents
{
    public class UIComponentButton : UIComponentPart
    {
        [SerializeField] private Button btn;

#if UNITY_EDITOR
        [OnInspectorInit]
        private void Search()
        {
            if (btn != null)
                return;
            btn = GetComponent<Button>();
        }
#endif
        public void PutData(UnityAction action)
        {
            btn.onClick.RemoveAllListeners();
            if (action != null)
                btn.onClick.AddListener(action);
        }

        private void OnDestroy()
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}