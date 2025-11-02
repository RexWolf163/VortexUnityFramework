using System.Collections.Generic;
using System.Linq;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SaveSystem;
using Vortex.Core.SaveSystem.Bus;

namespace Vortex.Core.DatabaseSystem.Bus
{
    public partial class Database : ISaveable
    {
        private const string SaveKey = "Database";

        public string GetSaveId() => SaveKey;

        public Dictionary<string, string> GetSaveData()
        {
            var list = _singletonRecords.Values.ToArray();
            var result = new Dictionary<string, string>();
            foreach (var record in list)
                result.AddNew(record.Guid, record.GetDataForSave());
            return result;
        }

        public void OnLoad()
        {
            var data = SaveController.GetData(SaveKey);
            foreach (var key in data.Keys)
            {
                //Если образца нет в БД, значит игнорируем его
                if (!_singletonRecords.ContainsKey(key))
                    continue;

                _singletonRecords[key].LoadFromSaveData(data[key]);
            }
        }
    }
}