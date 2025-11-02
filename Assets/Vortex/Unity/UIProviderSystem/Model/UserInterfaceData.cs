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
        internal Vector2 Offset = Vector2.zero;

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

        public override string GetDataForSave()
        {
            return $"{(int)Offset.x}-{(int)Offset.y}";
        }

        public override void LoadFromSaveData(string data)
        {
            var ar = data.Split('-');
            Offset = new Vector2(float.Parse(ar[0]), float.Parse(ar[1]));
        }
    }
}