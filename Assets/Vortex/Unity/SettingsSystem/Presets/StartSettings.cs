using Sirenix.OdinInspector;
using UnityEngine;

namespace Vortex.Unity.SettingsSystem.Presets
{
    public class StartSettings : SettingsPreset
    {
        [ValueDropdown("@DropDawnHandler.GetScenes()")] [SerializeField]
        private string startScene;

        public string StartScene => startScene;
    }
}