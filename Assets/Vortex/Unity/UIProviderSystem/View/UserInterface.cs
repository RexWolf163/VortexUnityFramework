using System;
using UnityEngine;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UI.Tweeners;
using Vortex.Unity.UI.UIComponents;
using Vortex.Unity.UIProviderSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model.Conditions;
using Vortex.Core.LocalizationSystem;

namespace Vortex.Unity.UIProviderSystem.View
{
    /// <summary>
    /// Реализация основы UI
    /// </summary>
    [Serializable]
    public sealed class UserInterface : MonoBehaviour
    {
        #region Params

        private const string TitlePostfix = "title";

        [SerializeField, DbRecord(typeof(UserInterfaceData))]
        private string preset;

        [SerializeField] private UIComponent title;

        private UserInterfaceData data;

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
            var titleText = $"{data.Name}_{TitlePostfix}".ToUpper().Translate();
            title.SetText(titleText);
            Check();
        }

        private void OnDisable()
        {
            UIProvider.Unregister(preset);
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