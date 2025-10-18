using System;
using System.Collections.Generic;
using Vortex.Core.AppLoading.Base.SystemController;

namespace Vortex.Core.AppLoading.Model
{
    public class AppLoaderModel
    {
        /// <summary>
        /// Реестр системных контроллеров, которые загружаются асинхронно
        /// Записываются типы наследников SystemController и ссылка на их представителя (агента, инстанс)
        /// </summary>
        internal Dictionary<Type, ISystemController> controllers = new();
    }
}