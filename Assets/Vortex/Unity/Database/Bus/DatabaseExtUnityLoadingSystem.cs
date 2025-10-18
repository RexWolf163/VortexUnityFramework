using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Database.Model.Record;
using Vortex.Extensions;

namespace Vortex.Database.Bus
{
    public partial class Database
    {
        private const string Path = "Database";

        private static Object[] _resources;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Run()
        {
            Register();
            _resources = Resources.LoadAll(Path);
        }

        public override async Task AgentLoadAsync(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            //FileSystemController.CreateFolders($"{Application.dataPath}/Resources/{_Path}");
#endif

            _loadingData.Size = _resources.Length;
            foreach (var resource in _resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _loadingData.Progress++;
                if (resource is not Record record)
                    continue;
                _records.AddNew(record.GetGuid(), record);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }

#if UNITY_EDITOR
        private void LoadDb()
        {
            var resources = Resources.LoadAll(Path);
            _records ??= new();
            _records.Clear();
            foreach (var resource in resources)
            {
                if (resource is not Record record)
                    continue;
                _records.AddNew(record.GetGuid(), record);
            }
        }
#endif
    }
}