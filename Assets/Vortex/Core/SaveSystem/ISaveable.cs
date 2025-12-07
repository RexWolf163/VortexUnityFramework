using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.System.ProcessInfo;

namespace Vortex.Core.SaveSystem
{
    /// <summary>
    /// Интерфейс контроллеров, чьи данные подлежат сохранению и загрузке
    /// </summary>
    public interface ISaveable
    {
        public string GetSaveId();

        public Task<Dictionary<string, string>> GetSaveData(CancellationToken cancellationToken);

        /// <summary>
        /// Указатель на данные процесса 
        /// </summary>
        /// <returns></returns>
        public ProcessData GetProcessInfo();

        /// <summary>
        /// Обработка события завершения загрузки
        /// </summary>
        public Task OnLoad(CancellationToken cancellationToken);
    }
}