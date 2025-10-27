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
        /// <param name="singletonRecords">линк на индекс синглтон-моделей</param>
        /// <param name="uniqRecords">линк на список id по которым могут быть представлены
        /// модели заполненные из пресета</param>
        public void SetIndex(SortedDictionary<string, Record> singletonRecords, HashSet<string> uniqRecords);

        /// <summary>
        /// Создает из пресета и возвращает новый экземпляр Record 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public T GetNewRecord<T>(string guid) where T : Record, new();
    }
}