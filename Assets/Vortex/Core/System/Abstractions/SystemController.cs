namespace Vortex.Core.System.Abstractions
{
    public abstract class SystemController<T, TD> : Singleton<T>
        where T : SystemController<T, TD>, new()
        where TD : ISystemDriver
    {
        protected static TD Driver;

        /// <summary>
        /// Установить реализацию
        /// </summary>
        /// <param name="driver"></param>
        /// <returns>TRUE - если слот драйвера был пустым, FALSE - если слот был полным. В этом случае
        /// запускаются процессы отключения старого драйвера и инициализации нового</returns>
        public static bool SetDriver(TD driver)
        {
            if (Driver != null && Driver.Equals(driver))
            {
                Driver.Destroy();
                Driver = driver;
                Instance.OnDriverConnect();
                Driver.Init();
                return false;
            }

            Driver = driver;
            Instance.OnDriverConnect();
            Driver.Init();
            return true;
        }

        /// <summary>
        /// Проверка наличия драйвера
        /// </summary>
        /// <returns></returns>
        public static bool HasDriver() => Driver != null;

        /// <summary>
        /// Обработка подключения нового драйвера
        /// </summary>
        protected abstract void OnDriverConnect();
    }
}