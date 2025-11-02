using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SettingsSystem.Bus;
using Vortex.Unity.UIProviderSystem.Model;
using Vortex.Unity.UIProviderSystem.Model.Conditions;

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
        /// Индекс зарегистрировавшихся UI по Id их пресета
        /// </summary>
        private static readonly SortedDictionary<string, UserInterfaceData> Uis = new();

        #endregion

        #region Public

        /// <summary>
        /// Регистрация нового интерфейса в индексе
        /// </summary>
        /// <param name="id"></param>
        internal static UserInterfaceData Register(string id)
        {
            var ui = Database.GetRecord<UserInterfaceData>(id);
            if (Settings.Data().AppStateDebugMode)
                Debug.Log($"[UIProvider] Registering UI : {ui.Name}");
            Uis.AddNew(id, ui);
            return ui;
        }

        /// <summary>
        /// Снятие с регистрации интерфейса
        /// </summary>
        /// <param name="id"></param>
        internal static void Unregister(string id)
        {
            if (!Uis.ContainsKey(id))
            {
                Debug.LogError($"[UIProvider] UI doesn't exist: {id}");
                return;
            }

            var ui = Uis.Get(id);
            if (Settings.Data().AppStateDebugMode)
                Debug.Log($"[UIProvider] UnRegistering UI : {ui.Name}");
            Uis.Remove(id);
        }

        #endregion
    }
}