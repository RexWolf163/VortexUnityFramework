using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.Loading;
using Vortex.Core.Loading.Controllers;
using Vortex.Core.Loading.SystemController;
using Vortex.Core.Database.Record;
using Vortex.Core.FileSystem;
using Vortex.Extensions;
using Object = UnityEngine.Object;

namespace Vortex.Database
{
    public partial class DatabaseController : ISystemController
    {
        private static readonly string _Path = "Database";

        private static Object[] resources;
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
            AppLoader.Register<DatabaseController>();
            resources = Resources.LoadAll(_Path);
        }

        public LoadingData GetLoadingData() => _loadingData;

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            FileSystemController.CreateFolders($"{Application.dataPath}/Resources/{_Path}");
#endif

            _loadingData.Size = resources.Length;
            foreach (var resource in resources)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _loadingData.Progress++;
                if (resource is not IDbRecord record)
                    continue;
                _records.AddNew(record.GetGuid(), record);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }


        public Type[] WaitingFor() => Type.EmptyTypes;

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