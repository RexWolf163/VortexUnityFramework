using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.MappedParametersSystem.Bus;
using Vortex.Core.System.ProcessInfo;
using Vortex.Unity.MappedParametersSystem.Base;
using Vortex.Unity.MappedParametersSystem.Base.Preset;

namespace Vortex.Unity.MappedParametersSystem
{
    public partial class MappedParametersDriver : IProcess
    {
        private ProcessData _processData;

        public ProcessData GetProcessInfo() => _processData;


        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            if (!MappedParameters.SetDriver(Instance))
            {
                Dispose();
                return;
            }

            Loader.Register(Instance);
        }


        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _processData = new ProcessData
            {
                Name = "Mapped Parameters",
                Progress = 1,
                Size = 1
            };
            /*
            var labels = Settings.Data().DatabaseLabels;

            if (labels == null || labels.Length == 0)
            {
                Debug.LogError(
                    "[DatabaseDriver] Метки (лейблы) не заданы в DatabaseSettings. Ассеты базы данных должны быть типа Addressable и помечены соответствующей меткой. Эти метки необходимо указать в DatabaseSettings.");
                return;
            }
            */

            /*
            var handles = new List<AsyncOperationHandle<IList<ParametersMap>>>();

            try
            {
                foreach (var label in labels)
                {
                    var handle = Addressables.LoadAssetsAsync<ParametersMap>(label, null);
                    handles.Add(handle);
                }

                _processData.Size = handles.Count;
                _processData.Progress = 0;

                foreach (var handle in handles)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var records = await handle.Task;

                    foreach (var map in records)
                        _indexMaps.AddNew(map.GetType().AssemblyQualifiedName, new MappedParametersGroup(map));

                    _processData.Progress++;
                    await Task.Yield();
                }
            }
            finally
            {
                foreach (var handle in handles)
                    Addressables.Release(handle);
            }
        */

            var resources = Resources.LoadAll("");
            _indexMaps.Clear();

            _processData.Size = resources.Length;
            foreach (var resource in resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _processData.Progress++;
                if (resource is not ParametersMap map)
                    continue;
                _indexMaps.AddNew(map.GetType().AssemblyQualifiedName, new MappedParametersGroup(map));

                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => null;
    }
}