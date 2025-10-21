using System;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Enums;

namespace Vortex.Core.AppSystem
{
    /// <summary>
    /// Интерфейс реализации App системы
    /// </summary>
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Запрос смены состояния
        /// </summary>
        public event Action<AppStates> CallStateChange;
    }
}