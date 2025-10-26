using Vortex.Core.DatabaseSystem.Model;

namespace Vortex.Unity.DatabaseSystem.Presets
{
    public interface IRecordPreset
    {
        /// <summary>
        /// Глобально уникальный идентификатор 
        /// </summary>
        public string Guid { get; }

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