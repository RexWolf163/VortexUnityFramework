using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Unity.UIProviderSystem.BehaviorLogics;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// Расширения получения данных
    /// </summary>
    public static partial class UIProvider
    {
        #region Public

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

        /// <summary>
        /// Вернуть все зарегистрированные интерфейсы
        /// </summary>
        /// <returns></returns>
        public static HashSet<UserInterface> GetAllUis() => _uis.Values.ToHashSet();

        /// <summary>
        /// Вернуть все зарегистрированные интерфейсы с указанным модулем поведения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HashSet<UserInterface> GetAllUis<T>() where T : UserInterfaceBehavior
        {
            return !_uisByBehaviour.ContainsKey(typeof(T))
                ? new HashSet<UserInterface>()
                : _uisByBehaviour[typeof(T)].Values.ToHashSet();
        }

        /// <summary>
        /// Вернуть все открытые интерфейсы
        /// </summary>
        /// <returns></returns>
        public static HashSet<UserInterface> GetAllOpenedUis()
        {
            var result = new HashSet<UserInterface>();
            var list = _uis.Values.ToArray();
            foreach (var userInterface in list)
            {
                if (userInterface.State is UserInterfaceStates.Hide or UserInterfaceStates.Hiding)
                    continue;
                result.Add(userInterface);
            }

            return result;
        }

        /// <summary>
        /// Вернуть все открытые интерфейсы с указанным модулем поведения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static HashSet<UserInterface> GetAllOpenedUis<T>() where T : UserInterfaceBehavior
        {
            var result = new HashSet<UserInterface>();
            if (!_uisByBehaviour.ContainsKey(typeof(T)))
                return result;
            var list = _uisByBehaviour[typeof(T)].Values.ToArray();
            foreach (var userInterface in list)
            {
                if (userInterface.State is UserInterfaceStates.Hide or UserInterfaceStates.Hiding)
                    continue;
                result.Add(userInterface);
            }

            return result;
        }

        #endregion
    }
}