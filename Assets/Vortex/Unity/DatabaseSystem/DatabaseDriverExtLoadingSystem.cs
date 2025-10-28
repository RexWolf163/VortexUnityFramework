using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.ProcessInfo;
using Vortex.Unity.DatabaseSystem.Presets;
using Object = UnityEngine.Object;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver : IProcess
    {
        private const string Path = "Database";

        /// <summary>
        /// Кешированный список ресурсов. Очищается после заполнения индексов
        /// </summary>
        private static Object[] _resources;

        /// <summary>
        /// Внутренний индекс пресетов
        /// </summary>
        private static SortedDictionary<string, IRecordPreset> _resourcesIndex = new();

        private ProcessData _processData = new()
        {
            Name = "Database",
            Progress = 0,
            Size = 0
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Register()
        {
            Database.SetDriver(Instance);
            Loader.Register<DatabaseDriver>();
            _resources = Resources.LoadAll(Path);
        }

        public ProcessData GetProcessInfo() => _processData;

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _recordsLink.Clear();
            _uniqRecordsLink.Clear();
            _resourcesIndex.Clear();
            _processData.Size = _resources.Length;
            foreach (var resource in _resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _processData.Progress++;
                if (resource is not IRecordPreset data)
                    continue;
                var record = data.GetData();

                _resourcesIndex.AddNew(data.Guid, data);
                AddRecord(record, data);

                await Task.Yield();
            }

            CallOnInit();
            _resources = null;
            await Task.CompletedTask;
        }

        private static void CallOnInit() => Instance.OnInit?.Invoke();

        public Type[] WaitingFor() => Type.EmptyTypes;
    }
}