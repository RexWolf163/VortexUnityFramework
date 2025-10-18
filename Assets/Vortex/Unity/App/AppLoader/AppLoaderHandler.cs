using UnityEngine;
using UnityEngine.SceneManagement;
using Vortex.Core.AppLoading.Bus;

namespace Vortex.Unity.Loading.Handlers
{
    public class AppLoaderHandler : MonoBehaviour
    {
        /// <summary>
        /// Предохранитель от мультивызова
        /// Как правило нужен при запуске из редактора
        /// </summary>
        private static bool _isRunning = false;
#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod]
        private static void Run()
        {
            var settings = Settings.Bus.Settings.Data();
            if (settings == null)
            {
                Debug.LogError(
                    "[AppLoaderHandler] SettingsController not initialized. Application start process is broken.");
                _isRunning = false;
                return;
            }

            SceneManager.LoadSceneAsync(settings.StartScene);
        }
#endif

        private void Awake()
        {
            //Защита от мультивызова при нарушении правил размещения или при вызове из редактора
            if (_isRunning)
                return;
            _isRunning = true;
            AppLoader.Run();
        }
    }
}