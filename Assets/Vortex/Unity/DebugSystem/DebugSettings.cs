using Sirenix.OdinInspector;
using UnityEngine;
using Vortex.Unity.SettingsSystem.Presets;

namespace Vortex.Unity.DebugSystem
{
    public partial class DebugSettings : SettingsPreset
    {
        [PropertyOrder(-100)]
        [SerializeField]
        [ToggleButtons(singleButton: true, trueColor: "@Color.red", falseColor: "@Color.green")]
        private bool debugMode = true;

        public bool DebugMode => debugMode;
    }
}