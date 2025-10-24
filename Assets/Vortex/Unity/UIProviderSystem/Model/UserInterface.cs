using System;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Tweeners;
using Vortex.Unity.UIProviderSystem.Bus;

namespace Vortex.Unity.UIProviderSystem.Model
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    [Serializable]
    public abstract partial class UserInterface : MonoBehaviour
    {
        #region Params

        /// <summary>
        /// Твиннеры открытия/закрытия
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        #endregion

        #region Private

        private void Awake()
        {
            behaviorLogic.Init(this);
        }

        private void OnDestroy()
        {
            behaviorLogic.DeInit();
        }

        protected void OnEnable()
        {
            _state = UserInterfaceStates.Hide;
            UIProvider.Register(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        protected void OnDisable()
        {
            UIProvider.Unregister(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        /// <summary>
        /// Открыть интерфейс
        /// </summary>
        internal void Open()
        {
            foreach (var tweener in tweeners)
            {
                tweener.Back(true);
                tweener.Forward();
            }

            SetState(UserInterfaceStates.Showing);
            TimeController.Call(() => SetState(UserInterfaceStates.Show), 0.3f,
                this); //TODO заменить Костыль
        }

        /// <summary>
        /// Закрыть интерфейс
        /// </summary>
        internal void Close()
        {
            foreach (var tweener in tweeners)
                tweener.Back();

            SetState(UserInterfaceStates.Hiding);
            TimeController.Call(() => SetState(UserInterfaceStates.Hide), 0.5f,
                this); //TODO заменить Костыль
        }

        #endregion

        /// <summary>
        /// Проверка открыт ли интерефейс
        /// </summary>
        /// <returns></returns>
        public bool IsOpened() => _state is UserInterfaceStates.Show or UserInterfaceStates.Showing;
    }
}