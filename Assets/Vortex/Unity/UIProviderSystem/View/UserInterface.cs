using System;
using UnityEngine;
using Vortex.Core.UIProviderSystem.Model;
using Vortex.Unity.DatabaseSystem.Attributes;
using Vortex.Unity.UI.Tweeners;
using UIProvider = Vortex.Core.UIProviderSystem.Bus.UIProvider;

namespace Vortex.Unity.UIProviderSystem.View
{
    /// <summary>
    /// Класс представление-контроллер интерфейса
    /// </summary>
    [Serializable]
    public sealed partial class UserInterface : MonoBehaviour
    {
        #region Params

        [SerializeField, DbRecord(typeof(UserInterfaceData))]
        private string preset;

        private UserInterfaceData _data;

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
        private bool _isOpen;

        private Vector2 Offset
        {
            get => new(_data.Offset.x, _data.Offset.y);
            set => _data.Offset = ((int)value.x, (int)value.y);
        }

        #endregion

        #region Private

        private void OnEnable()
        {
            _isOpen = false;
            foreach (var tweener in tweeners)
                tweener.Back(true);
            _data = UIProvider.Register(preset);
            _data.OnOpen += Check;
            _data.OnClose += Check;
            if (dragZone != null)
                dragZone.OnDrag += CalcPosition;
            _data.Init();
            Check();
        }

        private void OnDisable()
        {
            foreach (var condition in _data.Conditions)
                condition.DeInit();

            UIProvider.Unregister(preset);
            _data.DeInit();
            if (dragZone != null)
                dragZone.OnDrag -= CalcPosition;
            _data.OnOpen -= Check;
            _data.OnClose -= Check;
            _data = null;
            _isOpen = false;
            foreach (var tweener in tweeners)
                tweener.Back(true);
        }

        /// <summary>
        /// Выставление значения окна в нужное положение согласно данным модели
        /// </summary>
        private void Check()
        {
            if (_data.IsOpen)
                Open();
            else
                Close();
        }

        /// <summary>
        /// Проиграть анимацию открытия интерфейса
        /// </summary>
        private void Open()
        {
            if (_isOpen)
                return;
            SetPosition();
            foreach (var tweener in tweeners)
            {
                tweener.Back(true);
                tweener.Forward();
            }

            _isOpen = true;
        }

        /// <summary>
        /// Проиграть анимацию закрытия интерфейса
        /// </summary>
        private void Close()
        {
            if (!_isOpen)
                return;
            foreach (var tweener in tweeners)
                tweener.Back();
            _isOpen = false;
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