using System;
using System.Collections.Generic;
using Vortex.Core.Loading.SystemController;
using Vortex.Core.Enums;

namespace Vortex.Core.App
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
        private DateTime _startTime;

        /// <summary>
        /// Реестр системных контроллеров, которые загружаются асинхронно
        /// </summary>
        internal Dictionary<Type, ISystemController> controllers = new();

        internal AppModel()
        {
            _startTime = DateTime.UtcNow;
        }
    }
}