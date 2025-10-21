using Vortex.Core.System.ProcessInfo;

namespace Vortex.Core.SaveSystem
{
    public interface ISaveable : IProcess
    {
        public string GetSaveId();

        public string GetSaveData();

        /// <summary>
        /// Обработка события завершения загрузки
        /// </summary>
        public void OnLoad();
    }
}