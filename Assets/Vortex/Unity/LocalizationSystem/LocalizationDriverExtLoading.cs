using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.LocalizationSystem.Bus;
using Vortex.Core.System.ProcessInfo;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.LocalizationSystem.Presets;

namespace Vortex.Unity.LocalizationSystem
{
    public partial class LocalizationDriver : IProcess
    {
        private ProcessData _processData;

        private static LocalizationPreset _resource;

        public ProcessData GetProcessInfo() => _processData;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Register()
        {
            Localization.SetDriver(Instance);
            var resources = Resources.LoadAll<LocalizationPreset>(Path);
            if (resources == null || resources.Length == 0)
            {
                Debug.LogError("[Localization] Localization Preset not found]");
                return;
            }

            _resource = resources[0];
            Loader.Register<LocalizationDriver>();
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _localeData.Clear();
            var size = _resource.localeData.Length;
            _processData = new ProcessData()
            {
                Name = "Localization Data",
                Progress = 0,
                Size = size
            };

            var currentLanguage = Localization.GetCurrentLanguage().ToString();

            for (var i = 0; i < _resource.localeData.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                var data = _resource.localeData[i];
                var translateData = data.Texts.First(x => x.Language == currentLanguage);
                _localeData.AddNew(data.Key, translateData.Text);

                if (i % 20 == 0)
                    await Task.Yield();
            }

            TimeController.Accumulate(CallOnInit, this);
            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => null;
    }
}