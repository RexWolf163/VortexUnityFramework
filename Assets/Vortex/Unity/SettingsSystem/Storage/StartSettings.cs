using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.SettingsSystem.Storage
{
    [CreateAssetMenu(fileName = "StartSettings", menuName = "Settings/StartSettings")]
    public partial class StartSettings : SettingsStorage
    {
        [ValueDropdown("@SceneController.GetScenes()")] [SerializeField]
        private string startScene;

        public string StartScene => startScene;
    }
}