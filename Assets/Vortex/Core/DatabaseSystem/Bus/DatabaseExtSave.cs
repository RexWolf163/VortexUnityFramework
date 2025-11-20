using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.Utilities;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.SaveSystem;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.ProcessInfo;

namespace Vortex.Core.DatabaseSystem.Bus
{
    public partial class Database : ISaveable
    {
        private const string SaveKey = "Database";

        private static ProcessData _processData = new(name: SaveKey);

        public string GetSaveId() => SaveKey;

        public async Task<Dictionary<string, string>> GetSaveData(CancellationToken cancellationToken)
        {
            var list = _singletonRecords.Values.ToArray();
            var result = new Dictionary<string, string>();
            var counter = 0;
            foreach (var record in list)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return new Dictionary<string, string>();
                }

                var data = record.GetDataForSave();
                if (data.IsNullOrWhitespace())
                    continue;
                result.AddNew(record.GuidPreset, data);
                if (++counter != 20)
                    continue;
                counter = 0;
                await Task.Yield();
            }

            return result;
        }

        public ProcessData GetProcessInfo() => _processData;

        public async Task OnLoad(CancellationToken cancellationToken)
        {
            var data = SaveController.GetData(SaveKey);
            var counter = 0;
            foreach (var key in data.Keys)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                //Если образца нет в БД, значит игнорируем его
                if (!_singletonRecords.ContainsKey(key))
                    continue;

                _singletonRecords[key].LoadFromSaveData(data[key]);
                if (++counter != 1)
                    continue;
                counter = 0;
                await Task.Yield();
            }

            await Task.CompletedTask;
        }
    }
}