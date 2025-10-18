using System;
using UnityEngine;
using Vortex.Core.Enums;

namespace Vortex.Core.App.Model
{
    public partial class AppModel
    {
        /// <summary>
        /// Событие изменения состояния приложения
        /// </summary>
        public event Action<AppStates> OnStateChanged;

        /// <summary>
        /// Событие начала запуска приложения
        /// </summary>
        public event Action OnStarting;

        /// <summary>
        /// Системные контроллеры загружены, Приложение запущено
        /// </summary>
        public event Action OnStart;

        /// <summary>
        /// Событие начала выхода из приложения
        /// </summary>
        public event Action OnExit;

        public AppStates GetState() => _state;

        /// <summary>
        /// Выставить состояние приложения
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal bool SetState(AppStates state)
        {
            if (_state == state)
                return false;

#if UNITY_EDITOR
            Debug.Log($"AppState: {state}");
#endif
            var old = _state;
            _state = state;
            OnStateChanged?.Invoke(state);
            if (old == AppStates.Starting && _state != AppStates.Running)
                OnStart?.Invoke();
            if (_state == AppStates.Starting)
                OnStarting?.Invoke();
            if (_state == AppStates.Stopping)
                OnExit?.Invoke();
            return true;
        }
    }
}