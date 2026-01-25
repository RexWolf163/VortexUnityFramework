using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Vortex.Unity.UI.Attributes;
using Vortex.Unity.UI.StateSwitcher;

namespace Vortex.Unity.UI.Misc
{
    /// <summary>
    /// Класс "доработанной" кнопки.
    /// Транслирует события Нажал, Отпустил, Вошел в границы и вышел за границы
    /// </summary>
    public class AdvancedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        /// <summary>
        /// Способ фиксации кликов
        /// </summary>
        private enum ClickRegType
        {
            OnTap,
            OnUpInBorders,
            OnUpAnywhere
        }

        private enum ButtonState
        {
            Free,
            Hover,
            Pressed
        }

        /// <summary>
        /// Клик зарегистрирован согласно выбранной схеме
        /// </summary>
        public event Action OnClick;

        /// <summary>
        /// Нажатие на кнопку
        /// </summary>
        public event Action OnPressed;

        /// <summary>
        /// Кнопка отпущена
        /// </summary>
        public event Action OnReleased;

        /// <summary>
        /// Курсор над кнопкой
        /// </summary>
        public event Action OnHover;

        /// <summary>
        /// курсор покинул кнопку
        /// </summary>
        public event Action OnExit;

        [SerializeField] private UnityEvent[] onClick;
        [SerializeField] private UnityEvent[] onHover;
        [SerializeField] private UnityEvent[] onExit;

        [SerializeField] private ClickRegType clickRegType = ClickRegType.OnUpAnywhere;

        [SerializeField, StateSwitcher(typeof(ButtonState))]
        private UIStateSwitcher uiStateSwitcher;

        private bool pressed = false;

        private bool inBorders = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            inBorders = true;
            foreach (var act in onHover)
                act?.Invoke();

            Set(ButtonState.Hover);

            OnHover?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inBorders = false;
            foreach (var act in onExit)
                act?.Invoke();
            if (!pressed)
                Set(ButtonState.Free);

            OnExit?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
            if (clickRegType == ClickRegType.OnTap)
                Click();

            Set(ButtonState.Pressed);

            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
            if (clickRegType == ClickRegType.OnUpAnywhere || inBorders && clickRegType == ClickRegType.OnUpInBorders)
                Click();
            Set(inBorders ? ButtonState.Hover : ButtonState.Free);

            OnReleased?.Invoke();
        }

        private void Click()
        {
            foreach (var act in onClick)
                act?.Invoke();

            OnClick?.Invoke();
        }

        private void Set(ButtonState state) => uiStateSwitcher?.Set(state);

        /// <summary>
        /// Внешнее управление нажатием
        /// </summary>
        public void Press() => OnPointerDown(null);

        /// <summary>
        /// Внешнее управление нажатием
        /// </summary>
        public void Release() => OnPointerUp(null);
    }
}