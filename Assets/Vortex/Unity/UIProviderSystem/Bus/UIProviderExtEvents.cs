using System;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIProvider
    {
        /// <summary>
        /// Событие открытия интерфейса
        /// </summary>
        public static event Action<UserInterface> OnOpen;

        /// <summary>
        /// Событие закрытия интерфейса
        /// </summary>
        public static event Action<UserInterface> OnClose;
    }
}