using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.AppLoading.Base;
using Vortex.Core.AppLoading.Base.SystemController;

/// <summary>
/// Тестовый контроллер для проверки системы загрузки и индикации ее процесса 
/// </summary>
public class TestSystemController : SystemController<TestSystemController>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run() => Register();

    public Type[] WaitingFor() => null;

    public override LoadingData GetAgentLoadingData()
    {
        return _loadingData ??= new LoadingData
        {
            Name = "TestSystem",
            Progress = 0,
            Size = 100
        };
    }

    public override Type[] GetAgentWaitingFor() => null;

    public override async Task AgentLoadAsync(CancellationToken cancellationToken)
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
}