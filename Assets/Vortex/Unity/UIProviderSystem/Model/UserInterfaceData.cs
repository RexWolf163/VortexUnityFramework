using System;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Enums;
using Vortex.Unity.UIProviderSystem.Model.Conditions;

namespace Vortex.Unity.UIProviderSystem.Model
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

        public UserInterfaceCondition[] Conditions { get; protected set; }
        public UserInterfaceTypes UIType { get; set; }

        public void Init()
        {
            foreach (var condition in Conditions)
                condition.Init(this, CheckConditions);
            CheckConditions();
        }

        public void DeInit()
        {
            foreach (var condition in Conditions)
                condition.DeInit();
        }

        private void CheckConditions()
        {
            var state = IsOpen ? ConditionAnswer.Open : ConditionAnswer.Close;
            foreach (var condition in Conditions)
            {
                var answer = condition.Check();
                if (answer == ConditionAnswer.Idle)
                    continue;
                if (answer == ConditionAnswer.Open)
                {
                    state = ConditionAnswer.Open;
                    continue;
                }

                Close();
                return;
            }

            switch (state)
            {
                case ConditionAnswer.Open:
                    Open();
                    break;
                default:
                    Close();
                    break;
            }
        }

        public void Open()
        {
            if (IsOpen)
                return;
            IsOpen = true;
            OnOpen?.Invoke();
            UIProvider.CallOnOpen();
        }

        public void Close()
        {
            if (!IsOpen)
                return;
            IsOpen = false;
            OnClose?.Invoke();
            UIProvider.CallOnClose();
        }

        public override string GetDataForSave()
        {
            return $"{(int)Offset.x};{(int)Offset.y}";
        }

        public override void LoadFromSaveData(string data)
        {
            var ar = data.Split(';');
            Offset = new Vector2(float.Parse(ar[0]), float.Parse(ar[1]));
        }
    }
}