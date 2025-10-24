using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Tweeners;
using Vortex.Unity.UIProviderSystem.BehaviorLogics;
using Vortex.Unity.UIProviderSystem.Bus;

namespace Vortex.Unity.UIProviderSystem.Model
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    public abstract partial class UserInterface : MonoBehaviour
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
        private static UserInterfaceStates _state;

        /// <summary>
        /// Состояние окна
        /// </summary>
        public UserInterfaceStates State => _state;

        #endregion

        #region Private

        private void SetState(UserInterfaceStates state)
        {
            _state = state;
            OnStateChanged?.Invoke(_state);
        }

        #endregion

        #region Public

        /// <summary>
        /// Текущее состояние UI
        /// </summary>
        /// <returns></returns>
        public static UserInterfaceStates GetState() => _state;

        #endregion
    }
}