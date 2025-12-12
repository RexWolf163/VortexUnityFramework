using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Tweeners;

namespace AppScripts
{
    /// <summary>
    /// Хэндлер для мягкой выгрузки сцены прелоадера
    /// </summary>
    public class PreloadUnloadHandler : MonoBehaviour
    {
        /// <summary>
        /// Название сцены после загрузки которой активируется логика хэндлера
        /// </summary>
        [SerializeField, ValueDropdown("@DropDawnHandler.GetScenes()"),
         InfoBox("Название сцены после загрузки которой активируется логика хэндлера")]
        protected string sceneName;

        [SerializeField] private TweenerBase[] tweeners;

        /// <summary>
        /// Задает время на анимацию.
        /// После выбега выгружает сцену прелоадера
        /// </summary>
        [SerializeField, Range(0, 10f), InfoBox("Задает время на анимацию.\nПосле выбега выгружает сцену прелоадера")]
        private float delay = 2f;

        private void Awake()
        {
            foreach (var tweener in tweeners)
                tweener.Back(true);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            TimeController.RemoveCall(this);
        }

        public void OnSceneLoaded(Scene sceneName, LoadSceneMode loadSceneMode)
        {
            if (this.sceneName != sceneName.name)
                return;

            foreach (var tweener in tweeners)
                tweener.Forward();

            TimeController.Call(Unload, delay, this);
        }

        private void Unload() => SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }
}