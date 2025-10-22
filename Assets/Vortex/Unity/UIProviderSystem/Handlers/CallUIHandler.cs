using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Handlers
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
            Bus.UIProvider.CloseUI(type);
        }

        private void OpenUI()
        {
            Bus.UIProvider.OpenUI(type);
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
                    if (type.IsSubclassOf(typeof(UserInterface)))
                        result.Add(new ValueDropdownItem<string>(type.Name, type.AssemblyQualifiedName));
            }

            return result;
        }
#endif
    }
}