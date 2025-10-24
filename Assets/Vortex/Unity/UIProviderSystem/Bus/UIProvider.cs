using System;
using System.Collections.Generic;
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
        #region Params

        /// <summary>
        /// Индекс зарегистрировавшихся UI
        /// </summary>
        private static SortedDictionary<Type, UserInterface> _uis = new();

        #endregion

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

        /// <summary>
        /// Вернуть интерфейс указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static UserInterface GetUI<T>() where T : UserInterface
        {
            var wnd = _uis.Get(typeof(T));
            if (wnd == null)
            {
                Debug.LogError($"[UIController] No UI found for type {typeof(T).Name}");
                return null;
            }

            return wnd;
        }

        /// <summary>
        /// Вернуть интерфейс указанного типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UserInterface GetUI(Type type)
        {
            var wnd = _uis.Get(type);
            if (wnd == null)
            {
                Debug.LogError($"[UIController] No UI found for type {type.Name}");
                return null;
            }

            return wnd;
        }

        #endregion
    }
}