using System;
using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.DatabaseSystem;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SettingsSystem.Bus;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Abstractions.SystemControllers;
using Vortex.Unity.DatabaseSystem.Enums;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver : Singleton<DatabaseDriver>, IDriver
    {
        private static SortedDictionary<string, Record> _recordsLink;
        private static HashSet<string> _uniqRecordsLink;

        public event Action OnInit;

        /// <summary>
        /// Инициализация
        /// Запускается автоматически после назначения драйвера системе
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Передача указателя на реестр БД в драйвер для заполнения
        /// </summary>
        /// <param name="records"></param>
        /// <param name="uniqRecords"></param>
        public void SetIndex(SortedDictionary<string, Record> records, HashSet<string> uniqRecords)
        {
            _recordsLink = records;
            _uniqRecordsLink = uniqRecords;
        }

        /// <summary>
        /// Возвращает новый экземпляр записи заполненной по пресету с указанным id
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetNewRecord<T>(string guid) where T : Record, new()
        {
            if (!_resourcesIndex.ContainsKey(guid))
            {
                Debug.LogError($"Record with GUID \"{guid}\" does not exist.");
                return null;
            }

            var result = new T();
            var source = _resourcesIndex[guid];
            result.CopyFrom(source);
            return result;
        }

        public void Destroy()
        {
            if (Settings.Data().DebugMode)
                Debug.LogError("DatabaseDriver is destroyed");
        }

        /// <summary>
        /// Добавление записи в БД
        /// </summary>
        /// <param name="record"></param>
        /// <param name="data"></param>
        private static void AddRecord(Record record, IRecordPreset data)
        {
            if (record is null)
                Debug.LogError($"[DatabaseDriver] Can't load record: {data.Name} #{data.Guid}");
            else
            {
                switch (data.RecordType)
                {
                    case RecordTypes.Singleton:
                        _recordsLink.AddNew(record.Guid, record);
                        break;
                    case RecordTypes.MultiInstance:
                    default:
                        if (!_uniqRecordsLink.Add(record.Guid))
                            Debug.LogError($"Record with GUID \"{record.Guid}\" already exists.");

                        break;
                }
            }
        }
    }
}