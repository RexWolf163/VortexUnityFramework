using System.Collections.Generic;
using Vortex.Core.Database.Record;

namespace Vortex.Core.Database.Controllers
{
    public interface IDatabaseController
    {
        /// <summary>
        /// Получить запись из любой БД по GUID, приведенную к типу
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDbRecord GetRecord<T>(string guid) where T : class, IDbRecord;

        /// <summary>
        /// Получить все записи указанного типа из БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetRecords<T>() where T : class, IDbRecord;
    }
}