using System.Collections.Generic;
using Vortex.Core.Logger;
using Vortex.Database.Model.Record;

namespace Vortex.Database.Bus
{
    public partial class Database
    {
        /// <summary>
        /// Индекс записей в БД 
        /// </summary>
        private Dictionary<string, Record> _records = new();

        /// <summary>
        /// Получить запись по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static partial T GetRecord<T>(string guid) where T : Record;

        /// <summary>
        /// Получить все записи указанного типа из БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static partial List<T> GetRecords<T>() where T : Record;

        /// <summary>
        /// Получить запись по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T _GetRecord<T>(string guid) where T : Record
        {
            Instance._records.TryGetValue(guid, out var temp);
            if (temp == null)
                LogController.Log(new LogData(LogLevel.Error, $"Record not found for GUID: {guid}", Instance));
            var record = temp as T;
            if (record == null)
                LogController.Log(new LogData(LogLevel.Error, $"Record \"{guid}\" is not a {typeof(T).Name}",
                    Instance));
            return record;
        }

        /// <summary>
        /// Получить все записи указанного типа из БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> _GetRecords<T>() where T : Record
        {
            var result = new List<T>();
            var values = Instance._records.Values;
            foreach (var record in values)
            {
                if (record is not T item)
                    continue;
                result.Add(item);
            }

            return result;
        }
    }
}