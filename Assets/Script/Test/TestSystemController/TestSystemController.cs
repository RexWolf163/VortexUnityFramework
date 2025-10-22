using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Vortex.Core.LoaderSystem.Bus;
using Vortex.Core.System.ProcessInfo;

/// <summary>
/// Тестовый контроллер для проверки системы загрузки и индикации ее процесса 
/// </summary>
public class TestSystemController : IProcess
{
    [RuntimeInitializeOnLoadMethod]
    private static void Run() => Loader.Register<TestSystemController>();

    private ProcessData _processData = new()
    {
        Name = "TestSystem",
        Progress = 0,
        Size = 100
    };

    public ProcessData GetProcessInfo() => _processData;

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        for (var i = 1; i <= _processData.Size; i++)
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