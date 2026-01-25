using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.DatabaseSystem.Model.Enums;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions.SystemControllers;
using Vortex.Unity.DatabaseSystem.Presets;

namespace Vortex.Unity.DatabaseSystem.Drivers
{
    /// <summary>
    /// Общий для драйверов класс, предназначенный для соблюдения DRY
    /// Выполняет роль индекс-кеша для драйвера
    /// Должен использоваться исключительно рабочим драйвером (настроенным в конфигурационном файле)
    /// </summary>
    internal static class DatabaseDriverBase
    {
        private static Dictionary<string, Record> _recordsLink;
        private static HashSet<string> _multiInstanceRecordsLink;

        /// <summary>
        /// Внутренний индекс пресетов
        /// </summary>
        private static Dictionary<string, IRecordPreset> _resourcesIndex = new();

        /// <summary>
        /// Передача указателя на реестр БД в драйвер для заполнения
        /// </summary>
        /// <param name="records"></param>
        /// <param name="uniqRecords"></param>
        internal static void SetIndex(Dictionary<string, Record> records, HashSet<string> uniqRecords)
        {
            _recordsLink = records;
            _multiInstanceRecordsLink = uniqRecords;
        }

        /// <summary>
        /// Возвращает новый экземпляр записи заполненной по пресету с указанным id
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T GetNewRecord<T>(string guid) where T : Record, new()
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

        internal static void Clean()
        {
            _resourcesIndex.Clear();
            _recordsLink.Clear();
            _multiInstanceRecordsLink.Clear();
        }

        /// <summary>
        /// Добавление записи в БД
        /// </summary>
        /// <param name="record"></param>
        /// <param name="data"></param>
        internal static void AddRecord(Record record, IRecordPreset data)
        {
            if (record is null)
                Debug.LogError($"[DatabaseDriver] Can't load record: {data.Name} #{data.GuidPreset}");
            else
            {
                switch (data.RecordType)
                {
                    case RecordTypes.Singleton:
                        _recordsLink.AddNew(data.GuidPreset, record);
                        break;
                    case RecordTypes.MultiInstance:
                    default:
                        if (!_multiInstanceRecordsLink.Add(data.GuidPreset))
                            Debug.LogError($"Record with GUID \"{data.GuidPreset}\" already exists.");

                        break;
                }
            }
        }

        /// <summary>
        /// Добавить данные в индекс
        /// </summary>
        /// <param name="data"></param>
        public static void PutData(IRecordPreset data)
        {
            _resourcesIndex[data.GuidPreset] = data;
            AddRecord(data.GetData(), data);
        }
    }
}