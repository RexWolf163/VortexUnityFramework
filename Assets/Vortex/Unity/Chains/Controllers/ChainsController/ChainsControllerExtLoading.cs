using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Chains.Model;
using Vortex.Core.App;
using Vortex.Core.Loading;
using Vortex.Core.Loading.Controllers;
using Vortex.Core.Loading.SystemController;
using Vortex.Database;
using Vortex.Extensions;

namespace Vortex.Chains.Controllers
{
    public partial class ChainsController : ISystemController
    {
        private LoadingData _loadingData = new()
        {
            Name = "Chains",
            Progress = 0,
            Size = 0
        };

        [RuntimeInitializeOnLoadMethod]
        private static void Run()
        {
            AppLoader.Register<ChainsController>();
        }

        public LoadingData GetLoadingData() => _loadingData;

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            chains = new Dictionary<string, ChainData>();
            var db = AppController.GetSystem<DatabaseController>();
            var list = db.GetRecords<ChainData>();
            _loadingData.Size = list.Count;
            _loadingData.Progress = 0;
            foreach (var chain in list)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _loadingData.Progress++;
                chains.AddNew(chain.GetGuid(), chain);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => new[] { typeof(DatabaseController) };
    }
}