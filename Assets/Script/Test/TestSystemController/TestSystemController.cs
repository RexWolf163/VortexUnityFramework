using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.ProcessInfo;
using Vortex.Core.System.Loadable;

/// <summary>
/// Тестовый контроллер для проверки системы загрузки и индикации ее процесса 
/// </summary>
public class TestSystemController : IProcess
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run() => Loader.Register<TestSystemController>();

    private LoadingData _loadingData = new()
    {
        Name = "TestSystem",
        Progress = 0,
        Size = 100
    };

    public LoadingData GetProcessInfo() => _loadingData;

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        for (var i = 1; i <= _loadingData.Size; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.CompletedTask;
                return;
            }

            _loadingData.Progress++;
            await Task.Delay(50, cancellationToken);
            await Task.Yield();
        }

        await Task.CompletedTask;
    }

    public Type[] WaitingFor() => null;
}