using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.Loading;
using Vortex.Core.Loading.Controllers;
using Vortex.Core.Loading.SystemController;

/// <summary>
/// Тестовый контроллер для проверки системы загрузки и индикации ее процесса 
/// </summary>
public class TestSystemController : ISystemController
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run() => AppLoader.Register<TestSystemController>();

    private LoadingData _loadingData = new()
    {
        Name = "TestSystem",
        Progress = 0,
        Size = 100
    };

    public LoadingData GetLoadingData() => _loadingData;

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        for (var i = 1; i <= _loadingData.Size; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.CompletedTask;
                return;
            }

            _loadingData.Progress++;
            await Task.Delay(30);
            await Task.Yield();
        }

        await Task.CompletedTask;
    }

    public Type[] WaitingFor() => null;
}