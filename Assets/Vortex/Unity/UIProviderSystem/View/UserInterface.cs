using System;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UI.Tweeners;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.View
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    [Serializable]
    public sealed partial class UserInterface : MonoBehaviour
    {
        #region Params

        [SerializeField, DbRecord(typeof(UserInterfaceData))]
        private string preset;

        private UserInterfaceData data;

        /// <summary>
        /// Окно-Контейнер
        /// </summary>
        [SerializeField] private RectTransform wndContainer;

        /// <summary>
        /// Твиннеры открытия/закрытия
        /// </summary>
        [SerializeField] private TweenerBase[] tweeners;

        /// <summary>
        /// Флаг состояния представления
        /// </summary>
        private bool isOpen;

        #endregion

        #region Private

        private void OnEnable()
        {
            isOpen = false;
            foreach (var tweener in tweeners)
                tweener.Back(true);
            data = UIProvider.Register(preset);
            data.OnOpen += Check;
            data.OnClose += Check;
            if (dragZone != null)
                dragZone.OnDrag += CalcPosition;
            data.Init();
            Check();
        }

        private void OnDisable()
        {
            foreach (var condition in data.Conditions)
                condition.DeInit();

            UIProvider.Unregister(preset);
            data.DeInit();
            if (dragZone != null)
                dragZone.OnDrag -= CalcPosition;
            data.OnOpen -= Check;
            data.OnClose -= Check;
            data = null;
            isOpen = false;
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        /// <summary>
        /// Выставление значения окна в нужное положение согласно данным модели
        /// </summary>
        private void Check()
        {
            if (data.IsOpen)
                Open();
            else
                Close();
        }

        /// <summary>
        /// Проиграть анимацию открытия интерфейса
        /// </summary>
        private void Open()
        {
            if (isOpen)
                return;
            CalcPosition(data.Offset);
            foreach (var tweener in tweeners)
            {
                tweener.Back(true);
                tweener.Forward();
            }

            isOpen = true;
        }

        /// <summary>
        /// Проиграть анимацию закрытия интерфейса
        /// </summary>
        private void Close()
        {
            if (!isOpen)
                return;
            foreach (var tweener in tweeners)
                tweener.Back();
            isOpen = false;
        }

        #endregion

        #region Public

        /// <summary>
        /// Возвращает id интерфейса
        /// </summary>
        /// <returns></returns>
        public string GetId() => preset;

        #endregion
    }
}