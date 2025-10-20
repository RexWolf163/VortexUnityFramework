using System;
using System.Collections.Generic;
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

        protected override void OnDriverConnect()
        {
            OnSave = null;
            OnLoadStart = null;
            OnSaveStart = null;
            OnLoad = null;
            Driver.SetIndexLink(_saveDataIndex);
        }

        /// <summary>
        /// Запуск процедуры сохранения данных
        /// Если GUID не указан - сохранится под новым GUID
        /// </summary>
        /// <param name="guid"></param>
        public static void Save(string guid = null)
        {
            OnSaveStart?.Invoke();
            var list = OnSave?.Accumulate();
            if (list == null)
                return;
            _saveDataIndex.Clear();
            foreach (var item in list)
                _saveDataIndex.AddNew(item.Id, item.Data);

            guid ??= Crypto.GetNewGuid();
            Driver.Save(guid);
        }

        /// <summary>
        /// Загрузить сейв
        /// </summary>
        /// <param name="guid">guid сейва</param>
        public static void Load(string guid)
        {
            OnLoadStart?.Invoke();
            Driver.Load(guid);
            OnLoad?.Invoke();
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

        public static HashSet<string> GetIndex() => Driver.GetIndex();
    }
}