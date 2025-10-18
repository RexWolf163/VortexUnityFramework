﻿using System;

namespace Vortex.Core.App.Model
{
    public partial class AppModel
    {
        /// <summary>
        /// Возвращает время запуска
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartTime() => _startTime;
    }
}