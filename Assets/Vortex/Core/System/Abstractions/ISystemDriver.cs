namespace Vortex.Core.System.Abstractions
{
    public interface ISystemDriver
    {
        /// <summary>
        /// Инициализация
        /// Запускается автоматически после назначения драйвера системе
        /// </summary>
        public void Init();

        /// <summary>
        /// Уничтожение
        /// Запускается автоматически при отключении драйвера от системы
        /// </summary>
        public void Destroy();
    }
}