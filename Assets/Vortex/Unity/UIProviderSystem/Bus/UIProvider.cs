using UnityEngine;
using Vortex.Unity.UIProviderSystem.Enums;

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
                if (ui.Value.Type != UserInterfaceTypes.Common)
                    continue;
                ui.Value.Close();
            }
        }

        #endregion
    }
}