using System.Collections.Generic;
using System.Linq;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.DatabaseSystem.Bus
{
    public class Database : SystemController<Database, IDriver>
    {
        /// <summary>
        /// Реестр записей в БД 
        /// </summary>
        private SortedDictionary<string, Record> _records = new();

        /// <summary>
        /// Возвращает запись из БД по GUID приведенная к указанному типа
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRecord<T>(string guid) where T : Record, new()
        {
            if (!Instance._records.ContainsKey(guid))
            {
                Log.Print(
                    new LogData(LogLevel.Error, $"Record not found for GUID: {guid}", Instance));
                return null;
            }

            var record = Instance._records[guid] as T;
            if (record == null)
                Log.Print(
                    new LogData(LogLevel.Error, $"Record «{typeof(T).Name}» not found for GUID: {guid}", Instance));
            return record;
        }

        /// <summary>
        /// Возвращает запись из БД по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Record GetRecord(string guid)
        {
            if (!Instance._records.ContainsKey(guid))
            {
                Log.Print(
                    new LogData(LogLevel.Error, $"Record not found for GUID: {guid}", Instance));
                return null;
            }

            var record = Instance._records[guid];
            return record;
        }

        /// <summary>
        /// Возвращает все имеющиеся в реестре записи указанного типа
        /// </summary>
        /// <returns></returns>
        public static List<T> GetRecords<T>() where T : Record
        {
            var list = Instance._records.Values;
            var result = new List<T>();
            foreach (var record in list)
            {
                var tmp = record as T;
                if (tmp == null)
                    continue;
                result.Add(tmp);
            }

            return result;
        }

        /// <summary>
        /// Возвращает все имеющиеся в реестре записи
        /// </summary>
        /// <returns></returns>
        public static Record[] GetRecords() => Instance._records.Values.ToArray();

        /// <summary>
        /// Обработка подключения нового драйвера
        /// </summary>
        protected override void OnDriverConnect() => Driver.SetIndex(_records);

        /// <summary>
        /// Обработка отключения нового драйвера
        /// </summary>
        protected override void OnDriverDisonnect()
        {
            //Ignore
        }
    }
}