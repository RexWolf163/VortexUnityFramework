using System;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model.Conditions;

namespace Vortex.Unity.UIProviderSystem.Handlers
{
    public class CallUIHandler : MonoBehaviour
    {
        [SerializeField, DbRecord(typeof(UserInterfaceData))]
        protected string uiId;

        /// <summary>
        /// Закрывать вместо открытия
        /// </summary>
        [SerializeField] protected bool closeUI;

        [SerializeField] private UIComponent uiComponent;

        private Type type;

        private void Awake()
        {
            type = Type.GetType(uiId);
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
            UIProvider.Close(uiId);
        }

        private void OpenUI()
        {
            UIProvider.Open(uiId);
        }
    }
}