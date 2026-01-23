using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.SettingsSystem.Bus;
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
        private static Dictionary<string, IRecordPreset> _resourcesIndex = new();

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
            _resources = Resources.LoadAll(Path);
            var labels = Settings.Data().DatabaseLabels;
            if (labels == null || labels.Length == 0)
            {
                Debug.LogError(
                    "[DatabaseDriver] Метки (лейблы) не заданы в DatabaseSettings. Ассеты базы данных должны быть типа Addressable и помечены соответствующей меткой. Эти метки необходимо указать в DatabaseSettings.");
                return;
            }

            var ar = new List<IRecordPreset>();
            foreach (var label in labels)
            {
                var op = Addressables.LoadAssetsAsync<IRecordPreset>(label, null);
                var temp = op.WaitForCompletion();
                ar.AddRange(temp);
            }

            _resources = new Object[ar.Count];
            Array.Copy(ar.ToArray(), _resources, ar.Count);

            Loader.Register<DatabaseDriver>();
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

                _resourcesIndex.AddNew(data.GuidPreset, data);
                AddRecord(record, data);

                await Task.Yield();
            }

            CallOnInit();
            _resources = null;
            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => Type.EmptyTypes;
    }
}