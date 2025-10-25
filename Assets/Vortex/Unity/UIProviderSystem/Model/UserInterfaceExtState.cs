using System;
using UnityEngine;
using Vortex.Core.SettingsSystem.Bus;

namespace Vortex.Unity.UIProviderSystem.Model
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    public abstract partial class UserInterface
    {
        #region Events

        /// <summary>
        /// Событие изменения состояния окна
        /// </summary>
        public event Action<UserInterfaceStates> OnStateChanged;

        #endregion

        #region Params

        /// <summary>
        /// Состояние окна
        /// </summary>
        private UserInterfaceStates _state;

        /// <summary>
        /// Состояние окна
        /// </summary>
        public UserInterfaceStates State
        {
            get => _state;
            protected set
            {
                _state = value;
                if (Settings.Data().UiDebugMode)
                    Debug.Log($"{GetType().Name}: {State}");
            }
        }

        #endregion

        #region Private

        private void SetState(UserInterfaceStates state)
        {
            State = state;
            OnStateChanged?.Invoke(_state);
        }

        #endregion
    }
}