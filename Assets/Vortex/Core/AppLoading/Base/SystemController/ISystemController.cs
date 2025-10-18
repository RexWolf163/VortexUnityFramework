using System;
using System.Threading;
using System.Threading.Tasks;

namespace Vortex.Core.AppLoading.Base.SystemController
{
    /// <summary>
    /// Интерфейс агента-представителя контроллера
    /// </summary>
    public interface ISystemController
    {
        /// <summary>
        /// Данные загрузки
        /// </summary>
        public LoadingData GetAgentLoadingData();

        /// <summary>
        /// Перечень контроллеров "пропускаемых" вперед себя
        /// </summary>
        /// <returns></returns>
        public Type[] GetAgentWaitingFor();

        /// <summary>
        /// Процедура асинхронной загрузки
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AgentLoadAsync(CancellationToken cancellationToken);
    }
}