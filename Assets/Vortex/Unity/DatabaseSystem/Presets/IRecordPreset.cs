using Vortex.Core.DatabaseSystem.Model;
using Vortex.Unity.DatabaseSystem.Enums;

namespace Vortex.Unity.DatabaseSystem.Presets
{
    public interface IRecordPreset
    {
        /// <summary>
        /// Признак, определяющий как именно выдавать данные, в виде новой копии заполненной из пресета
        /// или ссылкой на класс в индексе 
        /// </summary>
        public RecordTypes RecordType { get; }

        /// <summary>
        /// Глобально уникальный идентификатор 
        /// </summary>
        public string GuidPreset { get; }

        /// <summary>
        /// Наименование элемента БД
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Возвращает модель данных заполненную из хранилища
        /// </summary>
        /// <returns></returns>
        public Record GetData();
    }
}