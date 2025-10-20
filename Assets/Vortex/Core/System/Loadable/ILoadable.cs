using System;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.System.Loadable;

namespace Vortex.Core.LoaderSystem.Loadable
{
    /// <summary>
    /// Интерфейс систем приложения
    /// Эти системы загружаются асинхронно и автоматически выстраивают свой порядок
    /// если их ответ на WaitingFor запрос корректен 
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Указатель на данные загрузки 
        /// </summary>
        /// <returns></returns>
        public LoadingData GetLoadingData();

        /// <summary>
        /// Запуск процедуры загрузки
        /// </summary>
        /// <returns></returns>
        public Task LoadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Перечень контроллеров "пропускаемых" вперед себя
        /// </summary>
        /// <returns></returns>
        public Type[] WaitingFor();
    }
}