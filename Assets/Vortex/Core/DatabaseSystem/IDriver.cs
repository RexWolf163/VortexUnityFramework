using System.Collections.Generic;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.DatabaseSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Передача указателя на реестр БД в драйвер для заполнения
        /// </summary>
        /// <param name="records"></param>
        public void SetIndex(SortedDictionary<string, Record> records);
    }
}