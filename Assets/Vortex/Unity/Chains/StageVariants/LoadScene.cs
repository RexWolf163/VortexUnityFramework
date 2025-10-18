using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vortex.Chains.Model.Stage
{
    /// <summary>
    /// Загрузка сцены
    /// </summary>
    public class LoadScene : ChainStage
    {
        /// <summary>
        /// Режим открытия сцены дополнительно
        /// </summary>
        [SerializeField] private bool additiveMode = true;

        [SerializeField, ValueDropdown("@SceneController.GetScenes()")]
        private string sceneName;

        protected override void Logic()
        {
            StartTask();
        }

        protected override void Stop()
        {
            //Ignore
        }

        private async Task StartTask()
        {
            var operation =
                SceneManager.LoadSceneAsync(sceneName, additiveMode ? LoadSceneMode.Additive : LoadSceneMode.Single);
            while (!operation.isDone)
            {
                await Task.Delay(500);
            }

            Complete();
        }
    }
}