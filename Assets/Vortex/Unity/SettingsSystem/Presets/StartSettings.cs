using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.SettingsSystem.Presets
{
    [CreateAssetMenu(fileName = "StartSettings", menuName = "Settings/StartSettings")]
    public partial class StartSettings : SettingsPreset
    {
        [ValueDropdown("@DropDawnHandler.GetScenes()")] [SerializeField]
        private string startScene;

        public string StartScene => startScene;
    }
}