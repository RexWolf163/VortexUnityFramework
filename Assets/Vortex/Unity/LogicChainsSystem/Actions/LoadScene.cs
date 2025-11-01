using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vortex.Unity.AppSystem.System.TimeSystem;

namespace Vortex.Unity.LogicChainsSystem.Actions
{
    public class LoadScene : UnityLogicAction
    {
        [SerializeField, ValueDropdown("@DropDawnHandler.GetScenes()")]
        protected string SceneName;

        [SerializeField] private bool _additiveMode;

        public override void Invoke()
        {
            TimeController.Call(() =>
                SceneManager.LoadSceneAsync(SceneName, _additiveMode ? LoadSceneMode.Additive : LoadSceneMode.Single));
        }

        protected override string NameAction =>
            $"Call load for «{(SceneName.IsNullOrWhitespace() ? "???" : SceneName)}» scene";
    }
}