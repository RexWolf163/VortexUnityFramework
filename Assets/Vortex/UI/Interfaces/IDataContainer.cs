namespace Vortex.UI
{
    public interface IDataContainer
    {
        /// <summary>
        /// Возвращает набор данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>();
    }
}