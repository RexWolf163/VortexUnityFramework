using System;
using System.Collections.Generic;
using System.Linq;
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
            if (wnd == null || wnd.IsOpened())
                return;
            wnd.Open();
            OnOpen?.Invoke(wnd);
        }

        /// <summary>
        /// Открыть UI указанного типа из указанной группы
        /// </summary>
        public static void OpenUI(Type type)
        {
            var wnd = GetUI(type);
            if (wnd == null || wnd.IsOpened())
                return;
            wnd.Open();
            OnOpen?.Invoke(wnd);
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CloseUI<T>() where T : UserInterface
        {
            var wnd = GetUI<T>();
            if (wnd == null || !wnd.IsOpened())
                return;
            wnd.Close();
            OnClose?.Invoke(wnd);
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        public static void CloseUI(Type type)
        {
            var wnd = GetUI(type);
            if (wnd == null || !wnd.IsOpened())
                return;
            wnd.Close();
            OnClose?.Invoke(wnd);
        }

        /// <summary>
        /// Закрыть все открытые UI
        /// </summary>
        public static void CloseAllUI()
        {
            var list = _uis.Values.Where(x => x.IsOpened()).ToArray();
            foreach (var ui in list)
                ui.Close();
            foreach (var ui in list)
                OnClose?.Invoke(ui);
        }

        /// <summary>
        /// Открыть все перечисленные UI
        /// </summary>
        /// <param name="uis"></param>
        public static void OpenUI(Type[] uis)
        {
            var list = new List<UserInterface>();
            foreach (var type in uis)
            {
                var wnd = GetUI(type);
                if (wnd == null || wnd.IsOpened())
                    return;
                wnd.Open();
                list.Add(wnd);
            }

            foreach (var wnd in list)
                OnOpen?.Invoke(wnd);
        }

        /// <summary>
        /// Закрыть все перечисленные UI
        /// </summary>
        /// <param name="uis"></param>
        public static void CloseUI(Type[] uis)
        {
            var list = new List<UserInterface>();
            foreach (var type in uis)
            {
                var wnd = GetUI(type);
                if (wnd == null || !wnd.IsOpened())
                    return;
                wnd.Close();
                list.Add(wnd);
            }

            foreach (var wnd in list)
                OnClose?.Invoke(wnd);
        }

        #endregion
    }
}