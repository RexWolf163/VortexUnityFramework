using System;
using System.Collections.Generic;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.UI.Interfaces;

namespace Vortex.Core.UI.Controllers
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIController
    {
        private static Dictionary<Type, IUserInterface> _uis = new();

        /// <summary>
        /// Открыть UI указанного типа из указанной группы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void OpenUI<T>() where T : IUserInterface
        {
            var wnd = GetUI<T>();
            wnd?.Open();
        }

        /// <summary>
        /// Открыть UI указанного типа из указанной группы
        /// </summary>
        public static void OpenUI(Type type)
        {
            var wnd = GetUI(type);
            wnd?.Open();
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CloseUI<T>() where T : IUserInterface
        {
            var wnd = GetUI<T>();
            wnd?.Close();
        }

        /// <summary>
        /// Закрыть UI указанного типа из указанной группы
        /// </summary>
        public static void CloseUI(Type type)
        {
            var wnd = GetUI(type);
            wnd?.Close();
        }

        /// <summary>
        /// Вернуть интерфейс указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IUserInterface GetUI<T>() where T : IUserInterface
        {
            var wnd = _uis.Get(typeof(T));
            if (wnd == null)
            {
                Log.Print(new LogData(LogLevel.Error, $"UI {typeof(T).Name} not found", "UIController"));
                return null;
            }

            return wnd;
        }

        /// <summary>
        /// Вернуть интерфейс указанного типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IUserInterface GetUI(Type type)
        {
            var wnd = _uis.Get(type);
            if (wnd == null)
            {
                Log.Print(new LogData(LogLevel.Error, $"UI {type.Name} not found", "UIController"));
                return null;
            }

            return wnd;
        }
    }
}