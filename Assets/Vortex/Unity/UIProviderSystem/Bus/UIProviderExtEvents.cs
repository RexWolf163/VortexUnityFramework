using System;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIProvider
    {
        #region Events

        /// <summary>
        /// Событие открытия окна
        /// </summary>
        public static event Action OnOpen;

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        public static event Action OnClose;

        #endregion

        #region Public

        internal static void CallOnOpen() => OnOpen?.Invoke();
        internal static void CallOnClose() => OnClose?.Invoke();

        #endregion
    }
}