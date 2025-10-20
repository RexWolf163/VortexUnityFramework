using Vortex.Core.SaveSystem.Abstraction;
using Vortex.Core.System.Loadable;

namespace Vortex.Core.System.Abstractions
{
    public interface ISaveable
    {
        public LoadingData GetLoadingData();
        public SaveData GetSaveData();

        /// <summary>
        /// Обработка события завершения загрузки
        /// </summary>
        public void OnLoad();
    }
}