using Vortex.Core.Enums;
using Vortex.Core.Loading.SystemController;
using Vortex.Core.Logger;

namespace Vortex.Core.App
{
    /// <summary>
    /// Шина-Контроллер приложения
    /// </summary>
    public static class AppController
    {
        private static AppModel _data;

        public static AppModel Data
        {
            get
            {
                if (_data == null)
                    Init();
                return _data;
            }
            private set { _data = value; }
        }

        /// <summary>
        /// Процедура первичной инициализации данных 
        /// </summary>
        private static void Init()
        {
            Data = new AppModel();
            Data.SetState(AppStates.Starting);
        }

        public static void Exit()
        {
            Data.SetState(AppStates.Stopping);
        }

        /// <summary>
        /// Получить указатель на системный контроллер (модуль)
        /// </summary>
        public static T GetSystem<T>() where T : class, ISystemController
        {
            var result = _data.controllers[typeof(T)] as T;
            if (result == null)
                LogController.Log(new LogData(LogLevel.Error, $"System \"{typeof(T).Name}\" not found",
                    "AppController"));
            return result;
        }
    }
}