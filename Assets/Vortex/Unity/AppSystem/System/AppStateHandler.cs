using System.Collections;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Enums;

namespace Vortex.Unity.AppSystem.System
{
    /// <summary>
    /// обработка перехода в фон и обратно
    /// Фиксирует "анфокус" только если состояние Run
    /// </summary>
    public class AppStateHandler : MonoBehaviour
    {
        private bool _pauseState;

        private AppStates _oldState;

#if UNITY_EDITOR
        /// <summary>
        /// Чекбокс для запуска из редактора
        /// </summary>
        private bool _started;
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            var go = Instantiate(new GameObject());
            go.AddComponent<AppStateHandler>();
            go.name = "AppStateHandler";
        }

        private void Awake()
        {
            _oldState = App.GetState();
            App.OnStateChanged += OnStateChanged;
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
            App.SetState(_pauseState ? AppStates.Unfocused : _oldState);
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
            if (!_started)
                return;
            StopAllCoroutines();
#endif
            App.Exit();
            App.OnStateChanged -= OnStateChanged;
        }

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            //Защита от процессов при запуске из редактора
            yield return new WaitForSeconds(1f);
            _started = true;
        }
#endif
    }
}