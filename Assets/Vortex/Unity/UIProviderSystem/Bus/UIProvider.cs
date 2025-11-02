using System.Collections.Generic;
using UnityEngine;
using Vortex.Unity.UIProviderSystem.Enums;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIProvider
    {
        #region Public

        public static void Open(string uiId)
        {
            if (!Uis.TryGetValue(uiId, out var ui))
            {
                Debug.LogError($"[UIProvider] UI doesn't exist: {uiId}");
                return;
            }

            ui.Open();
        }

        public static void Close(string uiId)
        {
            if (!Uis.TryGetValue(uiId, out var ui))
            {
                Debug.LogError($"[UIProvider] UI doesn't exist: {uiId}");
                return;
            }

            ui.Close();
        }

        /// <summary>
        /// Закрыть все базовые интерфейсы (не относится к вторичным типа панелей, оверлеев или попапов)
        /// </summary>
        public static void CloseAll()
        {
            foreach (var ui in Uis)
            {
                if (ui.Value.UIType != UserInterfaceTypes.Common)
                    continue;
                ui.Value.Close();
            }
        }

        /// <summary>
        /// Проверка наличия открытых Common интерфейсов
        /// </summary>
        /// <returns></returns>
        public static bool HasOpenedUIs()
        {
            var list = Uis.Values;
            foreach (var ui in list)
            {
                if (ui.UIType != UserInterfaceTypes.Common)
                    continue;
                if (ui.IsOpen)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Возвращает открытые Common интерфейсы
        /// </summary>
        /// <returns></returns>
        public static List<UserInterfaceData> GetOpenedUIs()
        {
            var list = Uis.Values;
            var result = new List<UserInterfaceData>();
            foreach (var ui in list)
            {
                if (ui.UIType != UserInterfaceTypes.Common)
                    continue;
                if (ui.IsOpen)
                    result.Add(ui);
            }

            return result;
        }

        #endregion
    }
}