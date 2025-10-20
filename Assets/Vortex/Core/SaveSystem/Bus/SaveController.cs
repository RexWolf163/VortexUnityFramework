using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.Extensions.LogicExtensions.Actions;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.SaveSystem.Abstraction;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.SaveSystem.Bus
{
    public class SaveController : SystemController<SaveController, IDriver>
    {
        private static readonly Dictionary<string, string> _saveDataIndex = new();

        /// <summary>
        /// Событие начала сохранения
        /// </summary>
        public static event Action OnLoadStart;

        /// <summary>
        /// Событие начала загрузки
        /// </summary>
        public static event Action OnSaveStart;

        /// <summary>
        /// Событие запроса данных для сохранения
        /// </summary>
        public static event Func<SaveData> OnSave;

        /// <summary>
        /// Событие завершения загрузки
        /// </summary>
        public static event Action OnLoad;

        protected override void OnDriverConnect() => Driver.SetIndexLink(_saveDataIndex);

        /// <summary>
        /// Запуск процедуры сохранения данных
        /// Если GUID не указан - сохранится под новым GUID
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="guid"></param>
        public static async Task Save(CancellationToken cancellationToken, string guid = null)
        {
            OnSaveStart?.Invoke();
            var list = OnSave?.Accumulate();
            if (list == null)
                return;
            _saveDataIndex.Clear();
            foreach (var item in list)
                _saveDataIndex.AddNew(item.Id, item.Json);

            guid ??= Crypto.GetNewGuid();
            await Task.Run(() => Driver.Save(guid, cancellationToken), cancellationToken);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Загрузить сейв
        /// </summary>
        /// <param name="guid">guid сейва</param>
        /// <param name="cancellationToken"></param>
        public static async void Load(string guid, CancellationToken cancellationToken)
        {
            OnLoadStart?.Invoke();
            await Task.Run(() => Driver.Load(guid, cancellationToken), cancellationToken);
            OnLoad?.Invoke();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Получить JSON данные по ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetData(string id)
        {
            if (_saveDataIndex.TryGetValue(id, out var data))
                return data;
            Log.Print(new LogData(LogLevel.Error, $"Save data not found for id: {id}", Instance));
            return "";
        }
    }
}