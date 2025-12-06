using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Tweeners;

namespace AppScripts
{
    public class PreloadUnloadHandler : MonoBehaviour
    {
        [SerializeField, ValueDropdown("@DropDawnHandler.GetScenes()")]
        protected string sceneName;

        [SerializeField, ValueDropdown("@DropDawnHandler.GetScenes()")]
        protected string preloaderSceneName = "preloader";

        [SerializeField] private TweenerBase[] tweeners;

        [SerializeField, Range(0, 10f)] private float delay = 2f;

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

        private void Unload()
        {
            SceneManager.UnloadSceneAsync(preloaderSceneName);
        }
    }
}