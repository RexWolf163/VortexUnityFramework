using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.AppLoading.Base;
using Vortex.Core.AppLoading.Base.SystemController;
using Vortex.Core.AppLoading.Model;
using Vortex.Core.Base;
using Vortex.Core.Enums;
using Vortex.Core.Logger;
using Vortex.Extensions;

namespace Vortex.Core.AppLoading.Bus
{
    /// <summary>
    /// Система загрузки приложения
    ///
    /// Загружаемые модули регистрируются через Register<T>() ДО начала загрузки
    /// Старт загрузки происходит по сигналу снаружи через Run() (зависит от платформы и реализации запуска)
    ///
    /// Порядок загрузки выстраивается автоматически по критерию "необходимой предзагрузки".
    /// То есть каждый ISystemController на запрос WaitingFor() должен вернуть перечень ISystemController,
    /// которые должны быть загружены вперед него
    /// </summary>
    public class AppLoader : Singleton<AppLoader>
    {
        /// <summary>
        /// Модель данных приложения
        /// </summary>
        private static AppLoaderModel _data;

        /// <summary>
        /// Модель данных приложения
        /// </summary>
        private static AppLoaderModel Data => _data ??= new AppLoaderModel();

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

        /// <summary>
        /// Регистрация в очереди на загрузку
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(T instance) where T : SystemController<T>, new()
        {
            var type = typeof(T);
            Data.controllers.AddNew(type, instance);
            _size = Data.controllers.Count;
        }

        /// <summary>
        /// Запуск процесса загрузки модулей
        /// </summary>
        public static async void Run()
        {
            App.Bus.App.Data.OnExit += Destroy;
            await Task.Run(() => Loading(Token));
            App.Bus.App.Data.SetState(AppStates.Running);
        }

        private static void Destroy()
        {
            App.Bus.App.Data.OnExit -= Destroy;
            CancelTokenSource.Cancel();
            CancelTokenSource.Dispose();
        }

        /// <summary>
        /// Запуск асинхронного процесса загрузки
        /// </summary>
        private static async Task Loading(CancellationToken token)
        {
            App.Bus.App.Data.SetState(AppStates.Starting);
            //Ждем все подписки
            var queue = Data.controllers.Values.ToList();
            var loaded = new HashSet<Type>();
            while (queue.Count > 0)
            {
                if (token.IsCancellationRequested)
                {
                    await Task.CompletedTask;
                    return;
                }

                ISystemController controller = null;
                var count = queue.Count;
                for (var i = 0; i < count; i++)
                {
                    var check = true;
                    controller = queue[i];
                    var waitFor = controller.GetAgentWaitingFor() ?? Array.Empty<Type>();
                    foreach (var type in waitFor)
                    {
                        if (!Data.controllers.ContainsKey(type))
                        {
                            LogController.Log(new LogData(LogLevel.Error, $"The expected сontroller {type} not found",
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

                        LogController.Log(new LogData(LogLevel.Error,
                            $"Loading critical error! Can not set order for next controllers: {sb}",
                            "AppLoader"));
                        return;
                    }
                    case null:
                        LogController.Log(new LogData(LogLevel.Error,
                            "Unknown error. Can not set order for next controllers",
                            "AppLoader"));
                        return;
                }

                _currentLoadingSystem = controller.GetAgentLoadingData() ?? new LoadingData
                {
                    Name = "Loading system",
                    Progress = 1,
                    Size = 1
                };
                _progress++;
                LogController.Log(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loading...",
                    "AppLoader"));
                await Task.Run(() => controller.AgentLoadAsync(token));
                loaded.Add(controller.GetType());
                LogController.Log(new LogData(LogLevel.Common,
                    $"{controller.GetType().Name}: loaded",
                    "AppLoader"));
            }

            LogController.Log(new LogData(LogLevel.Common,
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