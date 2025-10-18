using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.Loading;
using Vortex.Core.Loading.Controllers;
using Vortex.Core.Loading.SystemController;

namespace Vortex.Unity.Settings.Controllers
{
    public partial class SettingsController : ISystemController
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Run()
        {
            AppLoader.Register<SettingsController>();
        }

        public LoadingData GetLoadingData() => null;

        public async Task LoadAsync(CancellationToken cancellationToken) => await Task.CompletedTask;

        public Type[] WaitingFor() => Type.EmptyTypes;
    }
}