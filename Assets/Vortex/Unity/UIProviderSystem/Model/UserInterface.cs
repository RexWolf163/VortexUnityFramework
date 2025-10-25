using System;
using UnityEngine;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.System.Enums;
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

        protected void OnEnable()
        {
            State = UserInterfaceStates.Hide;
            foreach (var tweener in tweeners)
                tweener.Back(true);
            UIProvider.Register(this);
            behaviorLogic.Init(this);
        }

        protected void OnDisable()
        {
            behaviorLogic.DeInit();
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
        /// Проверка открыт ли интерфейс
        /// </summary>
        /// <returns></returns>
        public bool IsOpened() => _state is UserInterfaceStates.Show or UserInterfaceStates.Showing;
    }
}