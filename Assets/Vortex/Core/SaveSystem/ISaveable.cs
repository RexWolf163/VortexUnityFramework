using System.Collections.Generic;

namespace Vortex.Core.SaveSystem
{
    public interface ISaveable
    {
        public string GetSaveId();

        public Dictionary<string, string> GetSaveData();

        /// <summary>
        /// Обработка события завершения загрузки
        /// </summary>
        public void OnLoad();
    }
}