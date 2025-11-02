using System.Collections.Generic;
using Vortex.Core.System.Abstractions;

namespace Vortex.Core.SaveSystem
{
    public interface IDriver : ISystemDriver
    {
        /// <summary>
        /// Сохранить строки в сейв 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public void Save(string guid);

        /// <summary>
        /// Загрузить сейв
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public void Load(string guid);

        /// <summary>
        /// Передать линк на индекс
        /// </summary>
        public void SetIndexLink(Dictionary<string, Dictionary<string, string>> index);

        /// <summary>
        /// Возвращает все существующие сейвы
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetIndex();
    }
}