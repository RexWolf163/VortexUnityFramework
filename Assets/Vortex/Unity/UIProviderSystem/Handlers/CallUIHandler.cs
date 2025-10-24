using System;
using UnityEngine;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.Attributes;

namespace Vortex.Unity.UIProviderSystem.Handlers
{
    public class CallUIHandler : MonoBehaviour
    {
        [SerializeField, UserInterface] protected string uiType;

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
    }
}