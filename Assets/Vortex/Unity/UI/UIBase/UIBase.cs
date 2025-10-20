using System;
using UnityEngine;
using Vortex.Unity.AppSystem.System;
using Vortex.Unity.UI.Tweeners;

namespace Vortex.Core.UI.Interfaces
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    public abstract class UIBase : MonoBehaviour, IUserInterface
    {
        /// <summary>
        /// Состояния окна
        /// </summary>
        public enum WindowState
        {
            Hide,
            Showing,
            Show,
            Hiding,
        }

        /// <summary>
        /// Событие изменения состояния окна
        /// </summary>
        public event Action<WindowState> OnStateChanged;

        /// <summary>
        /// Твиннеры открытия/закрытия
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        /// <summary>
        /// Состояние окна
        /// </summary>
        private WindowState _state;

        /// <summary>
        /// Текущее состояние UI
        /// </summary>
        /// <returns></returns>
        public WindowState GetState() => _state;

        private void OnEnable()
        {
            Core.UI.Controllers.UIController.Register(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        private void OnDisable()
        {
            Core.UI.Controllers.UIController.Unregister(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        public void Open()
        {
            foreach (var tweener in tweeners)
            {
                tweener.Back(true);
                tweener.Forward();
            }

            OnStateChanged?.Invoke(WindowState.Showing);
            TimeController.Call(() => OnStateChanged?.Invoke(WindowState.Show), 0.3f, this); //Костыль
        }

        public void Close()
        {
            foreach (var tweener in tweeners)
                tweener.Back();

            OnStateChanged?.Invoke(WindowState.Hiding);
            TimeController.Call(() => OnStateChanged?.Invoke(WindowState.Hide), 0.5f, this); //Костыль
        }
    }
}