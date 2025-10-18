using System;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.AppLoading.Bus;
using Vortex.Core.Base;

namespace Vortex.Core.AppLoading.Base.SystemController
{
    /// <summary>
    /// система приложения
    /// Эти системы загружаются асинхронно и автоматически выстраивают свой порядок
    /// если их ответ на WaitingFor запрос корректен 
    /// </summary>
    public abstract class SystemController<T> : Singleton<T>, ISystemController where T : SystemController<T>, new()
    {
        protected static LoadingData _loadingData;
        public static LoadingData GetLoadingData() => _loadingData;

        /// <summary>
        /// Запуск процедуры загрузки
        /// </summary>
        /// <returns></returns>
        public static async Task LoadAsync(CancellationToken cancellationToken) =>
            await Instance.AgentLoadAsync(cancellationToken);


        /// <summary>
        /// Перечень контроллеров "пропускаемых" вперед себя
        /// </summary>
        /// <returns></returns>
        public static Type[] GetWaitingFor() => Instance.GetAgentWaitingFor();

        /// <summary>
        /// Данные загрузки
        /// </summary>
        public abstract LoadingData GetAgentLoadingData();

        /// <summary>
        /// Перечень контроллеров "пропускаемых" вперед себя
        /// </summary>
        /// <returns></returns>
        public abstract Type[] GetAgentWaitingFor();

        /// <summary>
        /// Процедура асинхронной загрузки
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task AgentLoadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Регистрация на загрузку
        /// </summary>
        protected static void Register() => AppLoader.Register(Instance);
    }
}