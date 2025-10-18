using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.UI.Components.UIComponents;

namespace Vortex.UI.Components
{
    public class CallUIHandler : MonoBehaviour
    {
        [SerializeField, ValueDropdown("GetList")]
        protected string uiType;

        /// <summary>
        /// Закрывать вместо открытия
        /// </summary>
        [SerializeField] protected bool closeUI;

        [SerializeField] private UIComponent uiComponent;

        private Type type;

        private void Awake()
        {
            type = Type.GetType(uiType);
            uiComponent?.SetAction(CallUI);
        }

        public virtual void CallUI()
        {
            if (closeUI)
                CloseUI();
            else
                OpenUI();
        }

        private void CloseUI()
        {
            UIController.CloseUI(type);
        }

        private void OpenUI()
        {
            UIController.OpenUI(type);
        }

#if UNITY_EDITOR
        private ValueDropdownList<string> GetList()
        {
            var result = new ValueDropdownList<string>();
            var currentDomain = AppDomain.CurrentDomain;
            var assems = currentDomain.GetAssemblies();
            foreach (var assembly in assems)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                    if (type.IsSubclassOf(typeof(UIBase)))
                        result.Add(new ValueDropdownItem<string>(type.Name, type.AssemblyQualifiedName));
            }

            return result;
        }
#endif
    }
}