using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.DatabaseSystem.Bus;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.LoaderSystem.Loadable;
using Vortex.Core.System.Loadable;
using Vortex.Unity.DatabaseSystem.Storage;
using Object = UnityEngine.Object;

namespace Vortex.Unity.DatabaseSystem
{
    public partial class DatabaseDriver : ILoadable
    {
        private const string Path = "Database";

        private static Object[] _resources;

        private LoadingData _loadingData = new()
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

        public LoadingData GetLoadingData() => _loadingData;

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            _recordsLink.Clear();
            _loadingData.Size = _resources.Length;
            foreach (var resource in _resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _loadingData.Progress++;
                if (resource is not IRecordStorage data)
                    continue;
                var record = data.GetData();
                AddRecord(record, data.Guid, data.Name);

                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => Type.EmptyTypes;
    }
}