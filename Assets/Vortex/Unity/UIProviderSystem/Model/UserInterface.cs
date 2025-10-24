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
    [Serializable]
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
        /// Скрипт поведения
        /// </summary>
        [SerializeReference, HideLabel,
         TabGroup("Behavior"),
         InfoBox("Укажите тип интерфейса!", InfoMessageType.Warning, VisibleIf = "ShowWarning"),
         GUIColor("GetBehaviorColor")]
        private UserInterfaceBehavior behaviorLogic;

        /// <summary>
        /// Твиннеры открытия/закрытия
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

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

        internal void Open()
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

        internal void Close()
        {
            foreach (var tweener in tweeners)
                tweener.Back();

            OnStateChanged?.Invoke(UserInterfaceStates.Hiding);
            TimeController.Call(() => OnStateChanged?.Invoke(UserInterfaceStates.Hide), 0.5f,
                this); //TODO заменить Костыль
        }

        /// <summary>
        /// Возвращает тип поведения UI
        /// </summary>
        /// <returns></returns>
        internal Type GetBehaviorType() => behaviorLogic.GetType();

        #endregion

        #region Public

        /// <summary>
        /// Текущее состояние UI
        /// </summary>
        /// <returns></returns>
        public static UserInterfaceStates GetState() => _state;

        #endregion

#if UNITY_EDITOR
        private string GetBehaviorName() => behaviorLogic?.GetType().Name;

        private bool ShowWarning() => behaviorLogic == null;

        private Color GetBehaviorColor() => behaviorLogic != null ? Color.yellow : Color.red;

        private ValueDropdownList<string> GetUIPanels() => new ValueDropdownList<string>();
#endif
    }
}