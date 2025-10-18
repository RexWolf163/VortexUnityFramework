using System;
using Vortex.Core.Enums;

namespace Vortex.Core.App.Model
{
    /// <summary>
    /// Модель данных приложения
    /// </summary>
    public sealed partial class AppModel
    {
        /// <summary>
        /// Состояние приложения
        /// </summary>
        private AppStates _state;

        /// <summary>
        /// отметка времени начала работы приложения
        /// </summary>
        private readonly DateTime _startTime;

        internal AppModel()
        {
            _startTime = DateTime.UtcNow;
        }
    }
}