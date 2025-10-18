using UnityEngine.SceneManagement;

namespace Vortex.Unity.UI.Components
{
    public class UnloadSceneHandler : SceneHandler
    {
        public override void Run() => SceneManager.UnloadSceneAsync(sceneName);
    }
}