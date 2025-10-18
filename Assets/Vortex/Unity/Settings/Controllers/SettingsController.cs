using UnityEngine;
using Vortex.Core.App;
using Vortex.Core.Settings;

namespace Vortex.Unity.Settings.Controllers
{
    public partial class SettingsController : IAppSettingsController
    {
#if UNITY_EDITOR
        /// <summary>
        /// Паттерн синглтона для доступа к данным из редактора
        /// </summary>
        private static SettingsController _instance;
#endif

        private static AppSettings _settings;

        private AppSettings GetData()
        {
            if (_settings == null)
            {
                var res = Resources.LoadAll("Settings");
                if (res is { Length: > 0 })
                    _settings = res[0] as AppSettings; //TODO подумать над мультифайловой структурой
            }

            if (_settings == null)
                Debug.LogError("[SettingsController] Settings not found");
            return _settings;
        }

        public static AppSettings Data()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                _instance ??= new SettingsController();
                return _instance.GetData();
            }
#endif
            var settings = AppController.GetSystem<SettingsController>();

            if (settings == null)
            {
                Debug.LogError("[AppLoaderHandler] SettingsController not initialized");
                return null;
            }

            return settings.GetData();
        }
    }
}