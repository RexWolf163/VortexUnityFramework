using System.Collections;
using UnityEngine;
using Vortex.Core.Enums;

namespace Vortex.Core.App
{
    /// <summary>
    /// обработка перехода в фон и обратно
    /// Фиксирует "анфокус" только если состояние Run
    /// </summary>
    public class AppFocusLogic : MonoBehaviour
    {
        private bool _pauseState;

        private AppStates _oldState;

#if UNITY_EDITOR
        /// <summary>
        /// Чекбокс для запуска из редактора
        /// </summary>
        private bool started;
#endif

        private void Awake()
        {
            _oldState = Bus.App.Data.GetState();
            Bus.App.Data.OnStateChanged += OnStateChanged;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _pauseState = !hasFocus;
            SetPauseState();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _pauseState = pauseStatus;
            SetPauseState();
        }

        private void SetPauseState()
        {
            Bus.App.Data.SetState(_pauseState ? AppStates.Unfocused : _oldState);
        }

        private void OnStateChanged(AppStates newState)
        {
            if (newState == AppStates.Unfocused)
                return;
            _oldState = newState;
        }

        /// <summary>
        /// Считается что разрушение этого компонента идет только при выходе из приложения
        /// </summary>
        private void OnDestroy()
        {
#if UNITY_EDITOR
            //Прерывание из-за проблем при запуске с активной сценой
            if (!started)
                return;
            StopAllCoroutines();
#endif
            Bus.App.Exit();
            Bus.App.Data.OnStateChanged -= OnStateChanged;
        }

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            //Защита от процессов при запуске из редактора
            yield return new WaitForSeconds(1f);
            started = true;
        }
#endif
    }
}