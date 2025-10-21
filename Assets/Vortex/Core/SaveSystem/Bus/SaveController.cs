using System;
using System.Collections.Generic;
using Vortex.Core.Extensions.LogicExtensions;
using Vortex.Core.LoggerSystem.Bus;
using Vortex.Core.LoggerSystem.Model;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.SaveSystem.Bus
{
    public class SaveController : SystemController<SaveController, IDriver>
    {
        private static readonly Dictionary<string, string> SaveDataIndex = new();

        private static readonly HashSet<ISaveable> Saveables = new();

        /// <summary>
        /// Событие начала сохранения
        /// </summary>
        public static event Action OnLoadStart;

        /// <summary>
        /// Событие начала загрузки
        /// </summary>
        public static event Action OnSaveStart;

        /// <summary>
        /// Событие завершения загрузки
        /// </summary>
        public static event Action OnLoadComplete;

        /// <summary>
        /// Событие завершения сохранения
        /// </summary>
        public static event Action OnSaveComplete;

        protected override void OnDriverConnect()
        {
            Driver.SetIndexLink(SaveDataIndex);
        }

        protected override void OnDriverDisonnect()
        {
            //Ignore
        }

        /// <summary>
        /// Запуск процедуры сохранения данных
        /// Если GUID не указан - сохранится под новым GUID
        /// </summary>
        /// <param name="guid"></param>
        public static void Save(string guid = null)
        {
            OnSaveStart?.Invoke();

            SaveDataIndex.Clear();
            foreach (var saveable in Saveables)
                SaveDataIndex.AddNew(saveable.GetSaveId(), saveable.GetSaveData());

            guid ??= Crypto.GetNewGuid();
            Driver.Save(guid);
            OnSaveComplete?.Invoke();
        }

        /// <summary>
        /// Загрузить сейв
        /// </summary>
        /// <param name="guid">guid сейва</param>
        public static void Load(string guid)
        {
            OnLoadStart?.Invoke();
            Driver.Load(guid);
            foreach (var saveable in Saveables)
                saveable.OnLoad();
            OnLoadComplete?.Invoke();
        }

        /// <summary>
        /// Получить JSON данные по ключу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetData(string id)
        {
            if (SaveDataIndex.TryGetValue(id, out var data))
                return data;
            Log.Print(new LogData(LogLevel.Error, $"Save data not found for id: {id}", Instance));
            return "";
        }

        public static HashSet<string> GetIndex() => Driver.GetIndex();

        public static void Register(ISaveable controller) => Saveables.Add(controller);

        public static void UnRegister(ISaveable controller) => Saveables.Remove(controller);
    }
}