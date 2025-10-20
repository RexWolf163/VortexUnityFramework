using System;
using System.Collections.Generic;
using Vortex.Core.LoaderSystem.Loadable;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.LoaderSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Добавить систему в список на загрузку
        /// </summary>
        /// <param name="type"></param>
        /// <param name="systemController"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool AddNew<T>(Type type, T systemController) where T : ILoadable;

        /// <summary>
        /// Кол-во зарегистрированных на загрузку систем
        /// </summary>
        /// <returns></returns>
        public int GetQueueLength();

        /// <summary>
        /// Список систем в очереди на загрузку
        /// </summary>
        /// <returns></returns>
        Dictionary<Type, ILoadable> GetQueue();
    }
}