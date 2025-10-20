using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoaderSystem.Loadable;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Enums;

namespace Vortex.Core.LoaderSystem.Bus
{
    /// <summary>
    /// Система загрузки приложения
    ///
    /// Загружаемые модули регистрируются через Register<T>() ДО начала загрузки
    /// Старт загрузки происходит по сигналу снаружи через Run() (зависит от платформы и реализации запуска)
    ///
    /// Порядок загрузки выстраивается автоматически по критерию "необходимой предзагрузки".
    /// То есть каждый ILoadable на запрос WaitingFor() должен вернуть перечень ILoadable,
    /// которые должны быть загружены вперед него
    /// </summary>
    public static class Loader
    {
        /// <summary>
        /// Прогресс загрузки
        /// </summary>
        private static int _progress;

        /// <summary>
        /// Общее кол-во шагов загрузки
        /// </summary>
        private static int _size;

        /// <summary>
        /// Данные загрузки текущего загружаемого модуля
        /// </summary>
        private static LoadingData _currentLoadingSystem;

        /// <summary>
        /// токен-ресурс прерывания
        /// </summary>
        private static readonly CancellationTokenSource CancelTokenSource = new();

        /// <summary>
        /// Токен прерывания
        /// </summary>
        private static CancellationToken Token => CancelTokenSource.Token;

        private static Dictionary<Type, ILoadable> _queue = new();

        /// <summary>
        /// Регистрация в очереди на загрузку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>() where T : ILoadable, new()
        {
            var type = typeof(T);
            _queue.AddNew(type, new T());
            _size = _queue.Count;
        }

        /// <summary>
        /// Запуск процесса загрузки модулей
        /// </summary>
        public static async void Run()
        {
            App.OnExit += Destroy;
            await Task.Run(() => Loading(Token));
            App.SetState(AppStates.Running);
        }

        private static void Destroy()
        {
            App.OnExit -= Destroy;
            CancelTokenSource.Cancel();
            CancelTokenSource.Dispose();
        }

        /// <summary>
        /// Запуск асинхронного процесса загрузки
        /// </summary>
        private static async Task Loading(CancellationToken token)
        {
            App.SetState(AppStates.Starting);
            //Ждем все подписки
            var queue = _queue.Values.ToList();
            var loaded = new HashSet<Type>();
            while (queue.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                ILoadable controller = null;
                var count = queue.Count;
                for (var i = 0; i < count; i++)
                {
                    var check = true;
                    controller = queue[i];
                    var waitFor = controller.WaitingFor() ?? Array.Empty<Type>();
                    foreach (var type in waitFor)
                    {
                        if (!_queue.ContainsKey(type))
                        {
                            Log.Print(new LogData(LogLevel.Error,
                                $"The expected сontroller {type} not found",
                                "AppLoader"));
                            continue;
                        }

                        if (!loaded.Contains(type))
                        {
                            check = false;
                            break;
                        }
                    }

                    if (check)
                    {
                        queue.RemoveAt(i);
                        break;
                    }
                }

                switch (controller)
                {
                    case null when queue.Count > 0:
                    {
                        var sb = new StringBuilder(queue[0].GetType().Name);
                        for (var j = 1; j < queue.Count; j++)
                        {
                            var systemController = queue[j];
                            sb.Append($", {systemController.GetType().Name}");
                        }

                        Log.Print(new LogData(LogLevel.Error,
                            $"Loading critical error! Can not set order for next controllers: {sb}",
                            "AppLoader"));
                        return;
                    }
                    case null:
                        Log.Print(new LogData(LogLevel.Error,
                            "Unknown error. Can not set order for next controllers",
                            "AppLoader"));
                        return;
                }

                _currentLoadingSystem = controller.GetLoadingData() ?? new LoadingData
                {
                    Name = "Loading system",
                    Progress = 1,
                    Size = 1
                };
                _progress++;
                Log.Print(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loading...",
                    "AppLoader"));
                await Task.Run(() => controller.LoadAsync(Token));
                loaded.Add(controller.GetType());
                Log.Print(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loaded",
                    "AppLoader"));
            }

            Log.Print(new LogData(LogLevel.Common,
                "Loading complete",
                "AppLoader"));
        }

        /// <summary>
        /// Прогресс загрузки
        /// </summary>
        /// <returns></returns>
        public static int GetProgress() => _progress;

        /// <summary>
        /// Общее кол-во шагов загрузки
        /// </summary>
        public static int GetSize() => _size;

        /// <summary>
        /// Данные загрузки текущего загружаемого модуля
        /// </summary>
        public static LoadingData GetCurrentLoadingData() => _currentLoadingSystem;
    }
}