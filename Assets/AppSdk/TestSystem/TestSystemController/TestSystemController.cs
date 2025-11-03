using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.SaveSystem;
using Vortex.Core.SaveSystem.Bus;
using Vortex.Core.System.ProcessInfo;

namespace AppSdk.TestSystem.TestSystemController
{
    /// <summary>
    /// Тестовый контроллер для проверки системы загрузки и индикации ее процесса 
    /// </summary>
    public class TestSystemController : IProcess, ISaveable
    {
        private static TestSystemController _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Run()
        {
            _instance = new TestSystemController();
            Loader.Register<TestSystemController>();
            SaveController.Register(_instance);
        }

        private ProcessData _processData = new()
        {
            Name = "TestSystem",
            Progress = 0,
            Size = 100
        };

        public string GetSaveId() => "TestSystem";

        public async Task<Dictionary<string, string>> GetSaveData(CancellationToken cancellationToken)
        {
            _processData.Progress = 0;
            for (var i = _processData.Size - 1; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return new Dictionary<string, string>();
                }

                _processData.Progress++;
                await Task.Delay(50, cancellationToken);
                await Task.Yield();
            }

            return new Dictionary<string, string>();
        }

        public ProcessData GetProcessInfo() => _processData;

        public async Task OnLoad(CancellationToken cancellationToken)
        {
            _processData.Progress = 0;
            for (var i = _processData.Size - 1; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _processData.Progress++;
                await Task.Delay(50, cancellationToken);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _processData.Progress = 0;
            for (var i = _processData.Size - 1; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                _processData.Progress++;
                await Task.Delay(50, cancellationToken);
                await Task.Yield();
            }

            await Task.CompletedTask;
        }

        public Type[] WaitingFor() => null;
    }
}