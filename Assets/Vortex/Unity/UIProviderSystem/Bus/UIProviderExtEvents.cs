using System;
using Vortex.Unity.UIProviderSystem.Model;

namespace Vortex.Unity.UIProviderSystem.Bus
{
    /// <summary>
    /// Контроллер-шина для работы с интерфейсами
    /// </summary>
    public static partial class UIProvider
    {
        public static event Action<UserInterface> OnOpen;
        public static event Action<UserInterface> OnClose;
    }
}