using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.AppSystem.Bus;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.System.ProcessInfo;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.SettingsSystem.Bus;
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
        #region Events

        /// <summary>
        /// Событие начала загрузки
        /// </summary>
        public static event Action OnLoad;

        /// <summary>
        /// Событие завершения загрузки
        /// </summary>
        public static event Action OnComplete;

        #endregion

        #region Params

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
        private static ProcessData _currentProcessSystem;

        /// <summary>
        /// токен-ресурс прерывания
        /// </summary>
        private static readonly CancellationTokenSource CancelTokenSource = new();

        /// <summary>
        /// Токен прерывания
        /// </summary>
        private static CancellationToken Token => CancelTokenSource.Token;

        /// <summary>
        /// Очередь загрузки
        /// </summary>
        private static readonly Dictionary<Type, IProcess> Queue = new();

        /// <summary>
        /// Защита от мультивызова
        /// </summary>
        private static bool _isRunning = false;

        #endregion

        #region Public

        /// <summary>
        /// Регистрация в очереди на загрузку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>() where T : IProcess, new()
        {
            if (Settings.Data().DebugMode)
                Log.Print(new LogData(LogLevel.Common, $"Register system: {typeof(T).Name}", "Loader"));
            var type = typeof(T);
            Queue.AddNew(type, new T());
            _size = Queue.Count;
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
        public static ProcessData GetCurrentLoadingData() => _currentProcessSystem;

        /// <summary>
        /// Запуск процесса загрузки модулей
        /// </summary>
        public static async Task Run()
        {
            if (_isRunning)
                return;
            OnLoad?.Invoke();
            _isRunning = true;
            App.OnExit += Destroy;
            if (Settings.Data().DebugMode)
            {
                var sb = new StringBuilder();
                foreach (var entry in Queue)
                    sb.Append(entry.Key.Name + "\n");
                Log.Print(new LogData(LogLevel.Common,
                    $"Loader running for {Queue.Count} systems\n<b>{sb}</b>",
                    "Loader"));
            }

            try
            {
                await Task.Run(() => Loading(Token));
            }
            catch (Exception ex)
            {
                Log.Print(new LogData(LogLevel.Error,
                    ex.Message + "\n" + ex.StackTrace,
                    "AppLoader"));
            }

            OnComplete?.Invoke();
            App.OnExit -= Destroy;
            App.SetState(AppStates.Running);
        }

        /// <summary>
        /// Запуск процесса одиночной загрузки отдельного модуля
        /// </summary>
        public static async Task RunAlone(IProcess controller)
        {
            _progress = 1;
            _size = 1;
            OnLoad?.Invoke();
            App.OnExit += Destroy;

            _currentProcessSystem = controller.GetProcessInfo() ?? new ProcessData
            {
                Name = "Loading system",
                Progress = 1,
                Size = 1
            };

            Log.Print(new LogData(LogLevel.Common,
                $"{controller.GetType().Name}: loading...",
                "AppLoader"));

            await Task.Run(() => controller.RunAsync(Token));

            Log.Print(new LogData(LogLevel.Common,
                "Loading complete",
                "AppLoader"));

            OnComplete?.Invoke();
            App.OnExit -= Destroy;
        }

        #endregion

        #region Private

        private static void Destroy()
        {
            App.OnExit -= Destroy;
            CancelTokenSource.Cancel();
        }

        /// <summary>
        /// Запуск асинхронного процесса загрузки
        /// </summary>
        private static async Task Loading(CancellationToken token)
        {
            App.SetState(AppStates.Starting);
            //Ждем все подписки
            var queue = Queue.Values.ToList();
            var loaded = new HashSet<Type>();
            while (queue.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                IProcess controller = null;
                var count = queue.Count;
                for (var i = 0; i < count; i++)
                {
                    var check = true;
                    controller = queue[i];
                    var waitFor = controller.WaitingFor() ?? Array.Empty<Type>();
                    foreach (var type in waitFor)
                    {
                        if (!Queue.ContainsKey(type))
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

                _currentProcessSystem = controller.GetProcessInfo() ?? new ProcessData
                {
                    Name = "Loading system",
                    Progress = 1,
                    Size = 1
                };
                _progress++;
                Log.Print(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loading...",
                    "AppLoader"));
                await Task.Run(() => controller.RunAsync(Token));
                loaded.Add(controller.GetType());
                Log.Print(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loaded",
                    "AppLoader"));
            }

            Log.Print(new LogData(LogLevel.Common,
                "Loading complete",
                "AppLoader"));
        }

        #endregion
    }
}