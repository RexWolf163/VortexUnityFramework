using System.Collections.Generic;
using UnityEngine;
using Vortex.Core.DatabaseSystem;
using Vortex.Core.DatabaseSystem.Model;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.Abstractions;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver : Singleton<DatabaseDriver>, IDriver
    {
        private SortedDictionary<string, Record> _recordsLink;

        public void Init()
        {
        }

        /// <summary>
        /// Передача указателя на реестр БД в драйвер для заполнения
        /// </summary>
        /// <param name="records"></param>
        public void SetIndex(SortedDictionary<string, Record> records) => _recordsLink = records;

        public void Destroy()
        {
            Debug.LogError("DatabaseDriver is destroyed");
        }

        /// <summary>
        /// Добавление записи в БД
        /// </summary>
        /// <param name="record"></param>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        private void AddRecord(Record record, string guid, string name)
        {
            if (record is null)
                Debug.LogError($"[DatabaseDriver] Can't load record: {name} #{guid}");
            else
                _recordsLink.AddNew(record.Guid, record);
        }
    }
}