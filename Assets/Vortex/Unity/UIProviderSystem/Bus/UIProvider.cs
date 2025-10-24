using System;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIProvider
    {
        #region Public

        /// <summary>
        /// Открыть UI указанного типа из указанной группы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void OpenUI<T>() where T : UserInterface
        {
            var wnd = GetUI<T>();
            OnOpen?.Invoke(wnd);
            wnd?.Open();
        }

        /// <summary>
        /// Открыть UI указанного типа из указанной группы
        /// </summary>
        public static void OpenUI(Type type)
        {
            var wnd = GetUI(type);
            OnOpen?.Invoke(wnd);
            wnd?.Open();
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CloseUI<T>() where T : UserInterface
        {
            var wnd = GetUI<T>();
            OnClose?.Invoke(wnd);
            wnd?.Close();
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        public static void CloseUI(Type type)
        {
            var wnd = GetUI(type);
            OnClose?.Invoke(wnd);
            wnd?.Close();
        }

        #endregion
    }
}