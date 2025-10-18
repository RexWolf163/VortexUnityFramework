using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.Database.Controllers;
using Vortex.Core.Database.Record;

namespace Vortex.Database
{
    public partial class DatabaseController : IDatabaseController
    {
#if UNITY_EDITOR
        private static DatabaseController _editorInstance;

        private static DatabaseController EditorInstance => _editorInstance ??= new DatabaseController();
#endif

        /// <summary>
        /// Индекс записей в БД 
        /// </summary>
        private Dictionary<string, IDbRecord> _records = new();

        /// <summary>
        /// Получить запись из любой БД по GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDbRecord GetRecord<T>(string guid) where T : class, IDbRecord
        {
            _records.TryGetValue(guid, out var record);
            if (record == null)
                Debug.LogError($"Record not found for GUID: {guid}");
            record = record as T;
            if (record == null)
                Debug.LogError($"Record \"{guid}\" is not a {typeof(T).Name}");
            return record;
        }

        /// <summary>
        /// Получить все записи указанного типа из БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetRecords<T>() where T : class, IDbRecord
        {
            var result = new List<T>();
            var values = _records.Values;
            foreach (var record in values)
            {
                if (record is not T item)
                    continue;
                result.Add(item);
            }

            return result;
        }

#if UNITY_EDITOR
        public static IDbRecord GetRecordStatic<T>(string guid) where T : class, IDbRecord
        {
            if (Application.isPlaying)
            {
                Debug.LogError("[DatabaseController] Call Editor function in play mode");
                return null;
            }

            EditorInstance.LoadDb();
            EditorInstance._records.TryGetValue(guid, out var record);
            if (record == null)
                Debug.LogError($"Record not found for GUID: {guid}");
            record = record as T;
            if (record == null)
                Debug.LogError($"Record \"{guid}\" is not a {typeof(T).Name}");
            return record;
        }

        public static List<IDbRecord> GetRecordsStatic<T>() where T : class, IDbRecord
        {
            if (Application.isPlaying)
            {
                Debug.LogError("[DatabaseController] Call Editor function in play mode");
                return null;
            }

            EditorInstance.LoadDb();
            var list = EditorInstance._records.Values;
            var result = new List<IDbRecord>();
            foreach (var record in list)
            {
                var tmp = record as T;
                if (tmp == null)
                    continue;
                result.Add(tmp);
            }

            return result;
        }
#endif
    }
}