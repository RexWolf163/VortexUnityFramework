using System;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Enums;

namespace Vortex.Unity.UIProviderSystem.Model.Conditions
{
    public class UserInterfaceData : Record
    {
        public event Action OnOpen;
        public event Action OnClose;

        /// <summary>
        /// точка якоря контейнера
        /// </summary>
        internal Vector2? Offset = null;

        internal bool IsOpen = false;

        public string Condition { get; protected set; }
        public UserInterfaceTypes Type { get; set; }

        public void Open()
        {
            IsOpen = true;
            OnOpen?.Invoke();
            UIProvider.CallOnOpen();
        }

        public void Close()
        {
            IsOpen = false;
            OnClose?.Invoke();
            UIProvider.CallOnClose();
        }
    }
}