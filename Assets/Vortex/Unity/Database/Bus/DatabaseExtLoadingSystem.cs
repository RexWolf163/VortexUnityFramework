using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.AppLoading.Base;
using Vortex.Core.AppLoading.Base.SystemController;
using Vortex.Core.AppLoading.Bus;

namespace Vortex.Database.Bus
{
    public partial class Database : SystemController<global::Unity.Database.Bus.Database>
    {
        private static readonly string _Path = "Database";

        private static Dictionary<string, string> _pathIndex = new();

        private LoadingData _loadingData = new()
        {
            Name = "Database",
            Progress = 0,
            Size = 0
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            AppLoader.Register<global::Unity.Database.Bus.Database>();
        }

        public LoadingData GetLoadingData() => _loadingData;

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            FileSystemController.CreateFolders($"{Application.dataPath}/Resources/{_Path}");
#endif

            var resources = Resources.LoadAll(_Path);

            _loadingData.Size = resources.Length;
            foreach (var resource in resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _loadingData.Progress++;
                if (resource is not DbRecord record)
                    continue;
                _records.AddNew(record.GetGuid(), record);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public override void SetLoadingData(string name, int progress, int size)
        {
            _loadingData.Name = name;
            _loadingData.Progress = progress;
            _loadingData.Size = size;
        }

        public override Type[] WaitingFor() => Type.EmptyTypes;

#if UNITY_EDITOR
        private void LoadDb()
        {
            resources = Resources.LoadAll(_Path);
            _records ??= new();
            _records.Clear();
            foreach (var resource in resources)
            {
                if (resource is not IDbRecord record)
                    continue;
                _records.AddNew(record.GetGuid(), record);
            }
        }
#endif
    }
}