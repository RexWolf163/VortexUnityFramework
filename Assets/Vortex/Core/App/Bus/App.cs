using Vortex.Core.Enums;

namespace Vortex.Core.App.Bus
{
    /// <summary>
    /// Шина-Контроллер данных приложения
    /// Здесь можно получить состояние приложения (например машина состояний приложения, время запуска)
    /// </summary>
    public static class App
    {
        private static Model.AppModel _data;

        public static Model.AppModel Data
        {
            get
            {
                if (_data == null)
                    Init();
                return _data;
            }
            private set => _data = value;
        }

        /// <summary>
        /// Процедура первичной инициализации данных 
        /// </summary>
        private static void Init()
        {
            Data = new Model.AppModel();
            Data.SetState(AppStates.Starting);
        }

        public static void Exit()
        {
            Data.SetState(AppStates.Stopping);
        }
    }
}