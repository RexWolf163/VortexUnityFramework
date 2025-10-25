using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SettingsSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// Функционал регистрации
    /// </summary>
    public static partial class UIProvider
    {
        #region Params

        /// <summary>
        /// Индекс зарегистрировавшихся UI
        /// </summary>
        private static Dictionary<Type, UserInterface> _uis = new();

        /// <summary>
        /// Индекс по типам поведения
        /// </summary>
        private static Dictionary<Type, Dictionary<Type, UserInterface>> _uisByBehaviour = new();

        #endregion

        #region Public

        /// <summary>
        /// Регистрация нового интерфейса в индексе
        /// </summary>
        /// <param name="ui"></param>
        internal static void Register(UserInterface ui)
        {
            var type = ui.GetType();
            var behType = ui.GetBehaviorType();

            if (Settings.Data().AppStateDebugMode)
                Debug.Log($"Registering UI : {type.Name}");
            _uis.AddNew(type, ui);
            if (!_uisByBehaviour.ContainsKey(behType))
                _uisByBehaviour.Add(behType, new Dictionary<Type, UserInterface>());
            _uisByBehaviour[behType].AddNew(type, ui);
        }

        /// <summary>
        /// Снятие с регистрации интерфейса
        /// </summary>
        /// <param name="ui"></param>
        internal static void Unregister(UserInterface ui)
        {
            var type = ui.GetType();
            if (Settings.Data().AppStateDebugMode)
                Debug.Log($"UnRegistering UI : {type.Name}");
            if (!_uis.ContainsKey(type))
            {
                Debug.LogError($"[UIController] UI doesn't exist: {type}");
                return;
            }

            var behType = ui.GetBehaviorType();

            _uis.Remove(type);
            _uisByBehaviour[behType].Remove(type);
            if (_uisByBehaviour[behType].Count == 0)
                _uisByBehaviour.Remove(behType);
        }

        #endregion
    }
}