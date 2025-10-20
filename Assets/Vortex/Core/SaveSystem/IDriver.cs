using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.SaveSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Сохранить строки в сейв 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Save(string guid, CancellationToken cancellationToken);

        /// <summary>
        /// Загрузить сейв
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Load(string guid, CancellationToken cancellationToken);

        /// <summary>
        /// Передать линк на индекс
        /// </summary>
        public void SetIndexLink(Dictionary<string, string> index);
    }
}