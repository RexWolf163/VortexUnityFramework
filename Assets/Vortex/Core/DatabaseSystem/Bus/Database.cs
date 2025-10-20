using System.Collections.Generic;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.DatabaseSystem.Bus
{
    public class Database : SystemController<Database, IDriver>
    {
        /// <summary>
        /// Индекс записей в БД 
        /// </summary>
        private SortedDictionary<string, Record> _records = new();

        public static T GetRecord<T>(string guid) where T : Record, new()
        {
            var record = Instance._records[guid] as T;
            if (record == null)
                Log.Print(
                    new LogData(LogLevel.Error, $"Record «{typeof(T).Name}» not found for GUID: {guid}", Instance));
            return record;
        }

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
        /// Обработка подключения нового драйвера
        /// </summary>
        protected override void OnDriverConnect() => Driver.SetIndex(_records);
    }
}