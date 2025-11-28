using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Vortex.Unity.UI.UIComponents.Parts
{
    public class UIComponentButton : UIComponentPart
    {
        [SerializeField] private Button btn;

        private UnityAction _currentAction;

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
            if (_currentAction != null)
                btn.onClick.RemoveListener(_currentAction);
            if (action != null)
                btn.onClick.AddListener(action);
        }

        private void OnDestroy()
        {
            if (_currentAction != null)
                btn.onClick.RemoveListener(_currentAction);
        }
    }
}