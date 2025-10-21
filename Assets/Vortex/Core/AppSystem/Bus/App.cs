using Vortex.Core.AppSystem.Model;
using Vortex.Core.System.Abstractions;
using Vortex.Core.System.Enums;
using NotImplementedException = System.NotImplementedException;

namespace Vortex.Core.AppSystem.Bus
{
    /// <summary>
    /// Шина-Контроллер приложения
    /// </summary>
    public partial class App : SystemController<App, IDriver>
    {
        private static AppModel _data;

        private static AppModel Data
        {
            get
            {
                if (_data != null)
                    return _data;

                _data = new AppModel();
                Init();

                return _data;
            }
        }

        /// <summary>
        /// Процедура первичной инициализации данных 
        /// </summary>
        private static void Init() => SetState(AppStates.Starting);

        /// <summary>
        /// Процедура завершения работы приложения 
        /// </summary>
        public static void Exit() => SetState(AppStates.Stopping);

        protected override void OnDriverConnect()
        {
            //Ignore
        }

        protected override void OnDriverDisonnect()
        {
            //Ignore
        }
    }
}