using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.AppSystem.System;
using Vortex.Unity.UI.Tweeners;

namespace Vortex.Unity.UIProviderSystem.Model
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    public abstract class UserInterface : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Событие изменения состояния окна
        /// </summary>
        public event Action<UserInterfaceStates> OnStateChanged;

        #endregion

        #region Params

        /// <summary>
        /// Твиннеры открытия/закрытия
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        [SerializeField, ValueDropdown("@DropDawnHandler.GetTypeList<IUserInterfaceBehavior>()")]
        private string behaviorLogic;

        /// <summary>
        /// Состояние окна
        /// </summary>
        private UserInterfaceStates _state;

        #endregion

        #region Private

        private void OnEnable()
        {
            Bus.UIProvider.Register(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        private void OnDisable()
        {
            Bus.UIProvider.Unregister(this);
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        #endregion

        #region Public

        /// <summary>
        /// Текущее состояние UI
        /// </summary>
        /// <returns></returns>
        public UserInterfaceStates GetState() => _state;

        public void Open()
        {
            foreach (var tweener in tweeners)
            {
                tweener.Back(true);
                tweener.Forward();
            }

            OnStateChanged?.Invoke(UserInterfaceStates.Showing);
            TimeController.Call(() => OnStateChanged?.Invoke(UserInterfaceStates.Show), 0.3f,
                this); //TODO заменить Костыль
        }

        public void Close()
        {
            foreach (var tweener in tweeners)
                tweener.Back();

            OnStateChanged?.Invoke(UserInterfaceStates.Hiding);
            TimeController.Call(() => OnStateChanged?.Invoke(UserInterfaceStates.Hide), 0.5f,
                this); //TODO заменить Костыль
        }

        #endregion
    }
}